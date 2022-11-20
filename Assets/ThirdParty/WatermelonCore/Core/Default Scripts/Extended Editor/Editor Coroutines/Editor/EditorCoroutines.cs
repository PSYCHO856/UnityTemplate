using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Watermelon
{
    public static class EditorCoroutines
    {
        private static readonly List<Coroutine> coroutines = new();

        public static void Execute(IEnumerator enumerator, Action<bool> OnUpdate = null)
        {
            if (coroutines.Count == 0) EditorApplication.update += Update;
            var coroutine = new Coroutine {enumerator = enumerator, OnUpdate = OnUpdate};
            coroutines.Add(coroutine);
        }

        private static void Update()
        {
            for (var i = 0; i < coroutines.Count; i++)
            {
                var coroutine = coroutines[i];
                var done = !coroutine.enumerator.MoveNext();
                if (done)
                {
                    if (coroutine.history.Count == 0)
                    {
                        coroutines.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        done = false;
                        coroutine.enumerator = coroutine.history[coroutine.history.Count - 1];
                        coroutine.history.RemoveAt(coroutine.history.Count - 1);
                    }
                }
                else
                {
                    if (coroutine.enumerator.Current is IEnumerator)
                    {
                        coroutine.history.Add(coroutine.enumerator);
                        coroutine.enumerator = (IEnumerator) coroutine.enumerator.Current;
                    }
                }

                if (coroutine.OnUpdate != null) coroutine.OnUpdate(done);
            }

            if (coroutines.Count == 0) EditorApplication.update -= Update;
        }

        public static void StopAll()
        {
            coroutines.Clear();
            EditorApplication.update -= Update;
        }

        private class Coroutine
        {
            public IEnumerator enumerator;
            public readonly List<IEnumerator> history = new();
            public Action<bool> OnUpdate;
        }
    }
}