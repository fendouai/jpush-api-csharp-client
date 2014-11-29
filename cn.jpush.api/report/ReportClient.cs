﻿using cn.jpush.api.common;
using cn.jpush.api.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cn.jpush.api.report
{
    class ReportClient:BaseHttpClient
    {
        private  const String REPORT_HOST_NAME = "https://report.jpush.cn";
        private  const String REPORT_RECEIVE_PATH = "/v2/received";
        private const String REPORT_RECEIVE_PATH_V3 = "/v3/received";

        private String appKey;
        private String masterSecret;

        public ReportClient(String appKey, String masterSecret) 
        {
            this.appKey = appKey;
            this.masterSecret = masterSecret;        
        }
        public ReceivedResult getReceiveds(String msg_ids) 
        {
            return getReceiveds_common(msg_ids, REPORT_RECEIVE_PATH);
        }
        public ReceivedResult getReceiveds_v3(String msg_ids)
        {
            return getReceiveds_common(msg_ids, REPORT_RECEIVE_PATH_V3);
        }
        private ReceivedResult getReceiveds_common(String msg_ids,string path)
        {
            String url = REPORT_HOST_NAME + path + "?msg_ids=" + msg_ids;
            String auth = Base64.getBase64Encode(this.appKey + ":" + this.masterSecret);
            ResponseWrapper rsp = this.sendGet(url, auth, null);
            ReceivedResult result = new ReceivedResult();
            List<ReceivedResult.Received> list = new List<ReceivedResult.Received>();

            Console.WriteLine("recieve content==" + rsp.responseContent);
            if (rsp.responseCode == System.Net.HttpStatusCode.OK)
            {
                list = (List<ReceivedResult.Received>)JsonTool.JsonToObject(rsp.responseContent, list);
                String content = rsp.responseContent;
            }
            result.ResponseResult = rsp;
            result.ReceivedList = list;
            return result;
        }
        //public MessagesResult getReportMessages(String[] msgIds)
        //{
            

        //}
        //public MessagesResult getReportMessages(params String msgIds)
        //{
            

        //}
        public MessagesResult getReportMessages(String msgIds)
        {
            checkMsgids(msgIds);
            throw new NotImplementedException();

        }
        public  void checkMsgids(String msgIds)
        {

            if (string.IsNullOrEmpty(msgIds)) {
                throw new ArgumentException("msgIds param is required.");
            }
            Regex reg = new Regex(@"[^0-9, ]");
            if(reg.IsMatch(msgIds))
            {
                  throw new ArgumentException("msgIds param format is incorrect. "
                        + "It should be msg_id (number) which response from JPush Push API. "
                        + "If there are many, use ',' as interval. ");
            }
            msgIds = msgIds.Trim();
            if (msgIds.EndsWith(",")) {
                msgIds = msgIds.Substring(0, msgIds.Length - 1);
            }

        
            String[] splits = msgIds.Split(',');
            try {
                foreach (String s in splits) {
                    string trim = s.Trim();
                    if (!string.IsNullOrEmpty(s)) {
                        int.Parse(trim);
                    }
                }
            } catch (Exception e) {
                throw new Exception("Every msg_id should be valid Integer number which splits by ','");
            }
        }
       
    }
}
