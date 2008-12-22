using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serenity.Web;
namespace Serenity.Net
{
    public class HttpServerAsyncState : ServerAsyncState
    {
        #region Fields
        private RequestProcessingStage stage;
        #endregion
        #region Methods
        public override void Reset()
        {
            this.Stage = RequestProcessingStage.None;
            base.Reset();
        }
        #endregion
        #region Properties
        public RequestProcessingStage Stage
        {
            get
            {
                return this.stage;
            }
            set
            {
                this.stage = value;
            }
        }
        #endregion
    }
}
