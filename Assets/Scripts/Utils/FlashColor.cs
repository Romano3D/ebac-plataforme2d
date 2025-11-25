using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlashColor : MonoBehaviour
{
    public List<SpriteRenderer> spriteRenderers;
    public Color color = Color.red;
    public float duration = .3f;

    private List<Tween> _tweens = new List<Tween>();

    private void OnValidate()
    {
        spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
    }

    public void Flash()
    {
        // Mata todos os tweens anteriores
        foreach (var t in _tweens)
        {
            if (t.IsActive()) t.Kill();
        }
        _tweens.Clear();

        // Reseta as cores
        foreach (var s in spriteRenderers)
        {
            if (s != null)
                s.color = Color.white;
        }

        // Cria novos tweens
        foreach (var s in spriteRenderers)
        {
            if (s != null)
            {
                var tween = s.DOColor(color, duration)
                             .SetLoops(2, LoopType.Yoyo);

                _tweens.Add(tween);
            }
        }
    }

    private void OnDestroy()
    {
        // garante que nenhum tween fica pendurado
        foreach (var t in _tweens)
        {
            if (t.IsActive()) t.Kill();
        }
    }
}
