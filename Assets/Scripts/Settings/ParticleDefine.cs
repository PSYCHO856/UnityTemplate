using UnityEngine;

[CreateAssetMenu(fileName = "ParticleDefine", menuName = "Settings/ParticleDefine")]
public class ParticleDefine : ScriptableObject
{
    [InspectorShow("完成时的特效")] public ParticleSystem completeParticle;

    [InspectorShow("结算特效")] public ParticleSystem settleParticle;
}