using BuildControllers;
using Managers;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlaceableFootprintDisplay : MonoBehaviour {
	
	//eventually this should do the wall sprite renderers too (to see wall space that is taken)
	private SpriteRenderer _spriteRenderer = new SpriteRenderer();
	public Sprite CompatableSprite;
	public Sprite IncompatableSprite;
	private Vector3 groundLift = new Vector3(0,0,.05f);

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.enabled = false;
	}

	
	/// <summary>
	/// Set the size of the grid underneeth and parent to the right spot
	/// </summary>
	public void InitializeUnderPlaceable(Placeable placeable)
	{
		transform.parent = placeable.RotatePivotPoint;
		transform.localPosition = Vector3.zero + groundLift;
		transform.localRotation = Quaternion.identity; 
		_spriteRenderer.enabled = true;
		_spriteRenderer.size = new Vector2(placeable.PlaceableData.Size.x, placeable.PlaceableData.Size.z);
	}
	
	public void ChangeStateToIncompatable()
	{
		_spriteRenderer.sprite = IncompatableSprite;
	}
	
	public void ChangeStateToCompatable()
	{
		_spriteRenderer.sprite = CompatableSprite;
	}

	public void RemoveFromPlaceable()
	{
		_spriteRenderer.enabled = false;
		transform.parent = GameMainManager.Instance.CurrentSceneManager.transform;
	}
}
