using System;
using System.Diagnostics;
using System.Text;

namespace querite
{
    internal class QueryAndLogDecorator<TModel, TSource> : QueryDecorator<TModel, TSource>
    {
        private TimeSpan _queryDuration;

        public QueryAndLogDecorator(IAmQuery<TModel, TSource> query)
            : base(query)
        {}

        protected override void OnBeforeApply()
        {
            NLog.LogManager.GetCurrentClassLogger(Query.GetType()).Log(NLog.LogLevel.Info, "About to run query for >> {0}", Query);
        }

        protected override void OnApply()
        {
            var timer = new Stopwatch();
            timer.Start();
            base.OnApply();
            timer.Stop();
            _queryDuration = timer.Elapsed;
        }

        protected override void OnAfterApply()
        {
            var message = new StringBuilder();
            message.AppendFormat("Query for >> {0} << completed. Execution Time: {1}", Query, _queryDuration);
            
            if (TotalRecordsReturned.HasValue)
                message.AppendFormat(", Total Records: {0}", TotalRecordsReturned);

            NLog.LogManager
                .GetCurrentClassLogger(Query.GetType())
                .Log(NLog.LogLevel.Info, message);
        }
    }
}