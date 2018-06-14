using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using OA.Model.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace OA.WebApp.Models
{
    public sealed class IndexManager
    {
        private static readonly IndexManager indexManager = new IndexManager();
        private IndexManager()
        {

        }
        public static IndexManager GetInstance()
        {
            return indexManager;
        }
        public Queue<ViewModelContent> queue = new Queue<ViewModelContent>();
        /// <summary>
        /// 要添加的数据入队，打上添加标记
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public void AddQueue(int id,string title,string content)
        {
            ViewModelContent viewModel = new ViewModelContent
            {
                Id = id,
                Title = title,
                Content = content,
                LuceneEnumType = LuceneEnumType.Add
            };
            queue.Enqueue(viewModel);
        
        }
        /// <summary>
        /// 要删除的数据id入队，打上删除标记
        /// </summary>
        /// <param name="id"></param>
        public void DeleteQueue(int id)
        {
            ViewModelContent viewModel = new ViewModelContent
            {
                Id = id,
                LuceneEnumType = LuceneEnumType.Delete
            };
            queue.Enqueue(viewModel);
        }
        /// <summary>
        /// 开始一个线程
        /// </summary>
        public void StartThread()
        {
            Thread thread = new Thread(WriteIndexContent);
            thread.IsBackground = true;
            thread.Start();
        }
        /// <summary>
        /// 检查队列中是否有数据，如果有数据就获取
        /// 没有就休息3秒
        /// </summary>
         private void WriteIndexContent()
        {
            if(queue.Count>0)
            {
                CreateIndex();
            }
            else
            {
                Thread.Sleep(3000);
            }
        }
        private void CreateIndex()
        {
            string indexPath = @"G:\lucenedir";//注意和磁盘上文件夹的大小写一致，否则会报错。将创建的分词内容放在该目录下。//将路径写到配置文件中。
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());//指定索引文件(打开索引目录) FS指的是就是FileSystem
            bool isUpdate = IndexReader.IndexExists(directory);//IndexReader:对索引进行读取的类。该语句的作用：判断索引库文件夹是否存在以及索引特征文件是否存在。
            if (isUpdate)
            {
                //同时只能有一段代码对索引库进行写操作。当使用IndexWriter打开directory时会自动对索引库文件上锁。
                //如果索引目录被锁定（比如索引过程中程序异常退出），则首先解锁（提示一下：如果我现在正在写着已经加锁了，但是还没有写完，这时候又来一个请求，那么不就解锁了吗？这个问题后面会解决）
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);//向索引库中写索引。这时在这里加锁。
            while(queue.Count>0)
            {
                ViewModelContent viewModel = queue.Dequeue();
                writer.DeleteDocuments(new Term("Id", viewModel.Id.ToString()));//删除
                if (viewModel.LuceneEnumType == LuceneEnumType.Delete)
                {
                    continue;
                }
                Document document = new Document();//表示一篇文档。
                                                   //Field.Store.YES:表示是否存储原值。只有当Field.Store.YES在后面才能用doc.Get("number")取出值来.Field.Index. NOT_ANALYZED:不进行分词保存

                document.Add(new Field("Id", viewModel.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                //Field.Index. ANALYZED:进行分词保存:也就是要进行全文的字段要设置分词 保存（因为要进行模糊查询）

                //Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS:不仅保存分词还保存分词的距离。
                document.Add(new Field("Title", viewModel.Title, Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                document.Add(new Field("Content", viewModel.Content, Field.Store.YES, Field.Index.ANALYZED, Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));
                writer.AddDocument(document);
            }
            writer.Close();//会自动解锁。
            directory.Close();//不要忘了Close
        }
    }
}