using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    public string id; // 唯一 ID，用于持久化（在 Inspector 中设置）
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isDone;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        // 从持久化管理器读取状态（若已存在）
        if (!string.IsNullOrEmpty(id) && PersistenceManager.Instance != null)
        {
            isDone = PersistenceManager.Instance.GetBool(id, isDone);
        }
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }
    public void TriggerAction()
    {
        // Debug.Log("Open Chest!");
        if (!isDone)
        {
            OpenChest();
        }
    }
    private void OpenChest()
    {
        spriteRenderer.sprite = openSprite;
        isDone = true;
        this.gameObject.tag = "Untagged";
        // 保存状态
        if (!string.IsNullOrEmpty(id) && PersistenceManager.Instance != null)
        {
            PersistenceManager.Instance.SetBool(id, true);
        }
        GetComponent<AudioDefinition>()?.PlayAudioClip();
    }
}
