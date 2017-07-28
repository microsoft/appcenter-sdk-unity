using System;
using System.Collections;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public class MobileCenterTask : CustomYieldInstruction
    {
        public delegate bool CompletionEvaluator();

        private readonly CompletionEvaluator _evaluator;

        public MobileCenterTask(CompletionEvaluator evaluator)
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

    public class MobileCenterTask<T> : MobileCenterTask
    {
        public delegate T ResultGetter();
        private readonly ResultGetter _resultGetter;

        public MobileCenterTask(CompletionEvaluator evaluator, ResultGetter resultGetter) : base(evaluator)
        {
            _resultGetter = resultGetter;
        }

        public T Result
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