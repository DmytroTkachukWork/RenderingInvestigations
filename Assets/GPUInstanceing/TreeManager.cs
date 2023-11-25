using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
  [SerializeField] private TreeLogicController tree_prefab = null;
  [SerializeField] private int trees_count = 100;
  [SerializeField] private Vector2 wood_size = Vector2.one;
  [SerializeField] private Vector2 rotation_range = Vector2.one;
  [SerializeField] private Vector2 scale_range = Vector2.one;
  [SerializeField] private RenderManager render_manager = null;

  private IEnumerator tree_spawn_cor = null;
  private List<TreeLogicController> trees_inst = new List<TreeLogicController>();
  void Start()
  {
    init();
  }

  private void init()
  {
    tree_spawn_cor = spawnTrees();
    StartCoroutine( tree_spawn_cor );
  }

  private void Update()
  {
    render_manager.drawFrame();
  }

  private IEnumerator spawnTrees()
  {
    int spawned_trees = 0;
    Vector3 new_tree_location = Vector3.zero;
    Vector3 new_rotation = Vector3.zero;
    Vector3 new_scale = Vector3.one;
    TreeLogicController new_tree = null;
    render_manager.startDrawTrees( trees_inst );

    while( spawned_trees < trees_count )
    {
      new_tree_location.x = Random.RandomRange( -wood_size.x, wood_size.x );
      new_tree_location.z = Random.RandomRange( -wood_size.y, wood_size.y );
      new_rotation.y = Random.RandomRange( rotation_range.x, rotation_range.y );
      new_scale.x = Random.RandomRange( scale_range.x, scale_range.y );
      new_scale.y = new_scale.x;
      new_scale.z = new_scale.x;

      new_tree = Instantiate( tree_prefab, new_tree_location, Quaternion.Euler(new_rotation) );
      new_tree.transform.localScale = new_scale;
      new_tree.init();
      new_tree.onClick += removeTree;
      trees_inst.Add( new_tree );
      spawned_trees++;

      yield return null;
    }
  }

  private void removeTree( TreeLogicController tree )
  {
    trees_inst.Remove( tree );
  }
}
