using UnityEngine;
using DG.Tweening;

public class UITween : MonoBehaviour
{

    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [SerializeField] private float duration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move()
    {
        transform.DOLocalMoveY(maxY, duration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DOLocalMoveY(minY, duration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                Move();
            });
        });

    }
}
