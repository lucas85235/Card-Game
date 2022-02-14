using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LifeMP : Life
{
    protected PhotonView m_view;

    protected override void Start()
    {
        base.Start();

        m_view = GetComponent<PhotonView>();
    }

    public override void TakeDamage(int decrement, AttackType type = AttackType.none, Element element = Element.normal, CardData usedCard = null, List<EffectSkill> skills = null)
    {
        if (m_view.IsMine)
        {
            m_view.RPC("TakeDamageRPC", RpcTarget.All, decrement, type, element, usedCard, skills);
        }
    }

    [PunRPC]
    public void TakeDamageRPC(int decrement, AttackType type = AttackType.none, Element element = Element.normal, CardData usedCard = null, List<EffectSkill> skills = null)
    {
        base.TakeDamage(decrement, type, element, usedCard, skills);
    }
}
