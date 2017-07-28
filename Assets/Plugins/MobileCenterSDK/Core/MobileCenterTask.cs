using System;
using System.Collections;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public class MobileCenterTask : CustomYieldInstruction
    {
        private readonly Func<bool> _evaluator;

        public MobileCenterTask(Func<bool> evaluator)
        {
            _evaluator = evaluator;
        }

        public override bool keepWaiting
        {
            get
            {
                return !IsDone;
            }
        }

        public bool IsDone
        {
            get
            {
                return _evaluator();
            }
        }
    }

    public class MobileCenterTask<TResult> : MobileCenterTask
    {
        private readonly Func<TResult> _resultGetter;

        public MobileCenterTask(Func<bool> evaluator, Func<TResult> resultGetter) : base(evaluator)
        {
            _resultGetter = resultGetter;
        }

        public TResult Result
        {
            get
            {
                if (!IsDone)
                {
                    throw new InvalidOperationException("Cannot get result until operation is complete");
                }
                return _resultGetter();
            }
        }
    }
}
