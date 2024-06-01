using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFruitController : MonoBehaviour
{
    public static ThrowFruitController instance;

    public GameObject CurrentFruit;
    [SerializeField] private Transform _fruitTransform;
    [SerializeField] private Transform _parentAfterThrow;
    [SerializeField] private FruitSelector _Selector;
    
    private PlayerController _playerController;

    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;

    public Bounds Bounds;

    private const float EXTRA_WIDTH = 0.02f;

    public bool CanThrow = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();

        SpawnAFruit(_Selector.PickRandomFruitForThrow());
    }

    private void Update()
    {
        if (PlayerInputManager.instance.isHoldingInput && CanThrow)
        {
            SpriteIndex index = CurrentFruit.GetComponent<SpriteIndex>();
            Quaternion rot = CurrentFruit.transform.rotation;

            GameObject go = Instantiate(FruitSelector.instance.Fruits[index.Index], CurrentFruit.transform.position, rot); 
            go.transform.SetParent(_parentAfterThrow);

            Destroy(CurrentFruit);

            CanThrow = false;
        }
        
    }

    public void SpawnAFruit(GameObject fruit)
    {
        GameObject go = Instantiate(fruit, _fruitTransform);
        CurrentFruit = go;
        _circleCollider = CurrentFruit.GetComponent<CircleCollider2D>();
        Bounds = _circleCollider.bounds;

        _playerController.ChangeBoundary(EXTRA_WIDTH);
    }
}
