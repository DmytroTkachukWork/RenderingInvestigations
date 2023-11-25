using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderManager : MonoBehaviour
{
  [SerializeField] private Mesh tree_mesh = null;
  [SerializeField] private Material tree_mat = null;
  [SerializeField] private int max_draws_in_batch = 100;
  [SerializeField] private bool draw_once = true;

  private List<TreeLogicController> trees_inst = new List<TreeLogicController>();
  private Matrix4x4[] transforms_matrix = null;
  private List<Matrix4x4> cached_transforms_matrix = new List<Matrix4x4>();

  public void startDrawTrees( List<TreeLogicController> trees_inst )
  {
    this.trees_inst = trees_inst;
  }

  public void drawFrame()
  {
    transforms_matrix = new Matrix4x4[trees_inst.Count];
    

   for ( int i = 0; i < trees_inst.Count; i++ )
     transforms_matrix[i] = trees_inst[i].transform_matrix;

    if ( draw_once )
    {
      Graphics.DrawMeshInstanced( tree_mesh, 0, tree_mat, transforms_matrix );
      return;
    }

    int flag_count = 0;

    while( flag_count < trees_inst.Count )
    {
      cached_transforms_matrix.Add( transforms_matrix[flag_count] );
      flag_count++;

      if ( cached_transforms_matrix.Count >= max_draws_in_batch )
      {
        Graphics.DrawMeshInstanced( tree_mesh, 0, tree_mat, cached_transforms_matrix );
        cached_transforms_matrix.Clear();
      }
    }

    Graphics.DrawMeshInstanced( tree_mesh, 0, tree_mat, cached_transforms_matrix );
    cached_transforms_matrix.Clear();
  }
}
