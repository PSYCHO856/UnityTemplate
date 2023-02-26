using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public static class RandomUtil {

        //include 0.0, 1.0
        public static float RandomUniform()
        {
            return UnityEngine.Random.Range(0.0f, 1.0f);
        }

        //rate 0-1
        public static bool RandomChance( float rate )
        {
            return RandomUniform()<=rate;
        }

        //include fmin, fmax
        public static float RandomFloat( float fmin, float fmax )
        {
            return UnityEngine.Random.Range( fmin, fmax );
        }

        //include imin, imax
        public static int RandomInt( int imin, int imax )
        {
            int result = Mathf.FloorToInt( RandomFloat(imin,imax+1.0f) );
            if ( result > imax ){
                result = imax;
            }
            return result;
        }

        /* 正态分布随机数,结果0-1， 期望为mu-决定中心位置，标准差为sigma-决定了分布的幅度 */
        public static float RandomBoxMuller( float mu = 0.5f, float sigma = 0.1667f )
        {
            float u = RandomUniform();
            float v = RandomUniform();
            float z = Mathf.Sqrt(-2*Mathf.Log(u)) * Mathf.Cos( 2*v*Mathf.PI );
            z = z*sigma + mu;
        //   --正态分布随机数   期望值为0.5  范围为0~1
		//   --对于正态分布，正负三个标准差之内的比率合起来为99%
		//  z = (z + 3) / 6;
            z = Mathf.Clamp01( z );
            return z;
        }

        //random from items list by weight, return item
        public static TKey RandomRate<TKey>( Dictionary<TKey, int> dictionary ){
            int total = 0;
            foreach (var keyValue in dictionary)
            {
                total += keyValue.Value;
            }
            if ( total < 1 ){
                Debug.LogWarning(" RandomRate total = 0 !!! ");
                total = 1;
            }

            int flag = RandomUtil.RandomInt( 1, total );

            foreach (var keyValue in dictionary)
            {
                flag -= keyValue.Value;
                if ( flag <= 0 )
                {
                    return keyValue.Key;
                }
            }
            
            return default;
        }

        //random from items list by weight, return items
        public static List<TKey> RandomRateMultiple<TKey>( Dictionary<TKey, int> dictionary, int count ){
            List<TKey> result = new List<TKey>();
            for ( int i = 0; i < count; i++ )
            {
                TKey key = RandomRate<TKey>( dictionary );
                result.Add(key);
            }
            
            return result;
        }

        //random from items list by weight, return items no repeat
        public static List<TKey> RandomRateMultipleNotRepeat<TKey>( Dictionary<TKey, int> dictionary, int count ){
            Dictionary<TKey, int> dictionaryNew = new Dictionary<TKey, int>(dictionary);
            List<TKey> result = new List<TKey>();
            for ( int i = 0; i < count; i++ )
            {
                TKey key = RandomRate<TKey>( dictionaryNew );
                result.Add(key);
                dictionaryNew[key] = 0;
            }
            
            return result;
        }

    }
}