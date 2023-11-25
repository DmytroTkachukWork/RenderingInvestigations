using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLogicController : MonoBehaviour
{
  [SerializeField] public Transform tree_root = null;
  [SerializeField] public Vector2 moving_bounds = Vector2.one;

  public Matrix4x4 transform_matrix = Matrix4x4.identity;
  private Vector2 moving_bounds_local = Vector2.one;

  public event Action<TreeLogicController> onClick = delegate{};

  public void init()
  {
    moving_bounds_local.x = tree_root.position.x + moving_bounds.x;
    moving_bounds_local.y = tree_root.position.z + moving_bounds.y;

    transform_matrix = Matrix4x4.TRS( tree_root.position, tree_root.rotation, tree_root.localScale );
    StartCoroutine( move() );
  }

  private void OnMouseDown()
  {
    onClick.Invoke(this);
    Destroy(this.gameObject);
  }

  private IEnumerator move()
  {
    while( true )
    {
      tree_root.position = new Vector3( tree_root.position.x, UnityEngine.Random.RandomRange( moving_bounds.x, moving_bounds.y ), tree_root.position.z );
      transform_matrix = Matrix4x4.TRS( tree_root.position, tree_root.rotation, tree_root.localScale );

      yield return null;
      yield return null;
      yield return null;
      yield return null;
    }
  }
}
