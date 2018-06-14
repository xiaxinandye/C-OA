using OA.BLL;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAQuartz
{
    public class BuildStasticsJob : IJob
    {
        private OA.IBLL.IT_SearchLogStasticsService T_SearchLogStasticsService; 
        public BuildStasticsJob()
        {
            T_SearchLogStasticsService = new T_SearchLogStasticsService();
        }
        public void Execute(JobExecutionContext context)
        {
            //删除原有的所有统计记录
            T_SearchLogStasticsService.DeleteAllStastics();
            //重新添加统计记录,只统计7天以内的热词
            T_SearchLogStasticsService.InsertAllStastics();
        }
    }
}
