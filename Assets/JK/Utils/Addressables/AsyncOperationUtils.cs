using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace JK.Utils.Addressables
{
    public static class AsyncOperationUtils
    {
        public static WaitUntil WaitUntilDone<T>(this AsyncOperationHandle<T> self)
        {
            return new WaitUntil(() => self.IsDone);
        }

        public static WaitUntil WaitUntilDone(this AsyncOperationHandle self)
        {
            return new WaitUntil(() => self.IsDone);
        }
    }
}