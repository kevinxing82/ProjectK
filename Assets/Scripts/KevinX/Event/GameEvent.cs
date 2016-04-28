using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KevinX.Event
{
    public class GameEvent
    {
        #region Property
        private int _eventType;
        private Object _source;
        private Object _paramObj;

        public delegate void CallBack(params object[] args);

        private CallBack _callbackFunc;
        private object[] _callbackArgs;

        private Dictionary<string, Object> _params = new Dictionary<string, object>();
        #endregion

        #region Constructor
        public GameEvent(Object source,int type)
        {
            this._source = source;
            this._eventType = type;
        }
        public GameEvent(Object source, Object paramObj,int type)
        {
            this._source = source;
            this._paramObj = paramObj;
            this._eventType = type;
        }
        #endregion

        #region Public function
        public int Type
        {
            get
            {
                return this._eventType;
            }
        }

        public Object Source
        {
           get
            {
                return this._source;
            }
        }

        public Dictionary<string,Object> Params
        {
            get
            {
                return this._params;
            }
            set
            {
                this._params = value;
            }
        }        

        public Object ParamObj
        {
            get
            {
                return this._paramObj;
            }
        }

        public CallBack CallBackFunc
        {
            get
            {
                return _callbackFunc;
            }
            set
            {
                this._callbackFunc = value;
            }
        }
        public object[] CallBackArgs
        {
            get
            {
                return _callbackArgs;
            }
            set
            {
                this._callbackArgs = value;
            }
        }
        #endregion

        #region Private function
        #endregion
    }
}
