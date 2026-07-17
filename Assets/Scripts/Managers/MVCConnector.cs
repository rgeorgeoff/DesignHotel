//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//
////this class connects the trifecta -- currently not used
//public class MVCConnector<TController, TView, TModel>
//{
//    List<MVCPairing<TController, TView, TModel>> connectionsList = new List<MVCPairing<TController, TView, TModel>>();
//
//    public MVCConnector(TController c, TView v, TModel m)
//    {
//    }
//
//    public MVCConnector(MVCPairing<TController, TView, TModel> m)
//    {
//    }
//
//
//    public class MVCPairing<TController, TView, TModel>
//    {
//        TController c;
//        TView v;
//        TModel m;
//
//        public MVCPairing(TController c, TView v, TModel m)
//        {
//            this.c = c;
//            this.v = v;
//            this.m = m;
//        }
//    }
//}