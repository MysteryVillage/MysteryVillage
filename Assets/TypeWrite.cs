using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Player;
using NPC;

public class TypeWrite : MonoBehaviour
{

    [SerializeField] private TMP_Text _target;
   // [SerializeField] private NPCData npc;
    private float _speed;
    private string _text;

    private int _curPosition = -1;
    public bool hasFinished { get; private set; }
  
    private TypeWrite(TMP_Text target,string text,float speed)
    {
        _target = target;
        _text = text;
        _speed = speed;
    }

    public static TypeWrite Start(TMP_Text target,string text,float speed = 0.1f)
    {
        var effect = new TypeWrite(target, text, speed);
        target.StartCoroutine(effect.Run());
        return effect;
    }
    private IEnumerator Run()
    {
        _target.text = "";

        var textLength = _text.Length;
        while(!hasFinished && _curPosition + 1 < textLength)
        {
            _target.text += GetNextToken();
            yield return new WaitForSeconds(_speed);
        }
        hasFinished = true;
    }
    public void Finish()
    {
        _target.StopCoroutine(Run());
        _target.text = _text;
        hasFinished = true;

    }
    private string GetNextToken()
    {
        _curPosition++;
        var nextToken = _text[_curPosition].ToString();
        return nextToken;
    }



}
