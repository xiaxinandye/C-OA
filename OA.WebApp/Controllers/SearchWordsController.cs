using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using OA.BLL;
using OA.Model;
using OA.WebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.WebApp.Controllers
{
    public class SearchWordsController : Controller
    {
        // GET: SearchWords
        IBLL.IBooksService BooksService { get; set; }
        IBLL.IT_SearchLogsService T_SearchLogsService { get;set;}
        IBLL.IT_SearchLogStasticsService T_SearchLogStasticsService { get; set; }
        public ActionResult Index()
        {
           List<T_SearchLogStastics> hotKeywords= T_SearchLogStasticsService.LoadEntities(w => true).Take(4).OrderByDescending(w => w.SearchCount).ToList();//将汇总表中搜索次数最多的4个词语返回
            ViewData["hotKeywords"] = hotKeywords;
            return View();
        }
        public ActionResult Search()
        {
            List<ViewModelContent> list = SearchWants();
            ViewData["ViewModelContent"] = list;
            return View("Index");
        }
        public ActionResult GetMatchWords()
        {
            string term = Request["term"];//获取用户搜索的词
            List<string> list=T_SearchLogStasticsService.GetMatchWords(term);//查表，返回相匹配的词语
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        private List<ViewModelContent> SearchWants()
        {
            string indexPath = @"G:\lucenedir";
            List<string> list = Common.WebCommon.PanGuSplitWord(Request["txtSearch"]);//对用户输入的搜索条件进行拆分。
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            //搜索条件
            PhraseQuery queryTitle = new PhraseQuery();
            foreach (string word in list)//先用空格，让用户去分词，空格分隔的就是词“计算机   专业”
            {
                queryTitle.Add(new Term("Title", word));
                T_SearchLogsService.AddEntity(new T_SearchLogs {
                    Id = Guid.NewGuid(),
                    Word = word,
                    SearchDate = DateTime.Now
                });//将用户搜索的关键词插入到明细表中
            }
            //query.Add(new Term("body","语言"));--可以添加查询条件，两者是add关系.顺序没有关系.
            // query.Add(new Term("body", "大学生"));
            // query.Add(new Term("body", kw));//body中含有kw的文章
            queryTitle.SetSlop(100);//多个查询条件的词之间的最大距离.在文章中相隔太远 也就无意义.（例如 “大学生”这个查询条件和"简历"这个查询条件之间如果间隔的词太多也就没有意义了。）
                                    //TopScoreDocCollector是盛放查询结果的容器
            PhraseQuery queryContent = new PhraseQuery();
            foreach (string word in list)//先用空格，让用户去分词，空格分隔的就是词“计算机   专业”
            {
                queryContent.Add(new Term("Content", word));
            }
            queryContent.SetSlop(100);
            BooleanQuery query = new BooleanQuery();//总查询条件
            query.Add(queryTitle, BooleanClause.Occur.SHOULD);
            query.Add(queryContent, BooleanClause.Occur.SHOULD);
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            searcher.Search(query, null, collector);//根据query查询条件进行查询，查询结果放入collector容器
            ScoreDoc[] docs = collector.TopDocs(0, collector.GetTotalHits()).scoreDocs;//得到所有查询结果中的文档,GetTotalHits():表示总条数   TopDocs(300, 20);//表示得到300（从300开始），到320（结束）的文档内容.
            //可以用来实现分页功能
            List<ViewModelContent> viewModelList = new List<ViewModelContent>();
            for (int i = 0; i < docs.Length; i++)
            {
                //
                //搜索ScoreDoc[]只能获得文档的id,这样不会把查询结果的Document一次性加载到内存中。降低了内存压力，需要获得文档的详细内容的时候通过searcher.Doc来根据文档id来获得文档的详细内容对象Document.
                ViewModelContent viewModel = new ViewModelContent();
                int docId = docs[i].doc;//得到查询结果文档的id（Lucene内部分配的id）
                Document doc = searcher.Doc(docId);//找到文档id对应的文档详细信息
                viewModel.Id = Convert.ToInt32(doc.Get("Id"));// 取出放进字段的值
                viewModel.Content = Common.WebCommon.CreateHightLight(Request["txtSearch"], doc.Get("Content"));
                viewModel.Title = Common.WebCommon.CreateHightLight(Request["txtSearch"], doc.Get("Title"));

                viewModelList.Add(viewModel);
            }
            return viewModelList;
        }


    }
}