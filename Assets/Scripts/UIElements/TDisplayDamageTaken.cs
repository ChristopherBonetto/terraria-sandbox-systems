using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Terraria.AI;

public class TDisplayDamageTaken : MonoBehaviour
{
    [SerializeField] private float m_LifeTime;
    private float m_CurrentTime;

    private Text m_Text;

    private void Awake()
    {
        m_Text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        TEventManager.SubscribeTo<IDefend>(TEventID.OnHealthUpdate, SetTextBasedOnDamageTaken);

        m_CurrentTime = m_LifeTime;
    }

    private void OnDisable()
    {
        
        TEventManager.UnsubscribeFrom<IDefend>(TEventID.OnHealthUpdate, SetTextBasedOnDamageTaken);
    }

    private void Update()
    {
        m_CurrentTime -= Time.deltaTime;

        if (m_CurrentTime < 0)
            gameObject.SetActive(false);
    }

    public void SetTextBasedOnDamageTaken(IDefend defenseComponent)
    {
        if (defenseComponent is TEnemyDefenseComponent)
            m_Text.text = "-" + (defenseComponent.LastHealth - defenseComponent.CurrentHealth).ToString();
    }
}
