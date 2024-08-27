using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainManagers;
using Other;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animation))]
public class PlayerMovement : MonoBehaviour
{
   public Texture2D cursorNormal;
   public Texture2D cursorClosed;

   private Rigidbody2D _rb;

   private float _horizontal;
   private float _vertical;

   public ClothesPair bodySprites;
   public ClothesPair hairSprites;
   public Color hairMultiplyColor;

   public float runSpeed = 5.0f;
   [HideInInspector] public bool isMoving;
   public bool canMove;
   public bool isInBuilding;

   [NamedArrayAttribute(new string[] { "Grass", "Dirt", "Rock" })]
   public AudioCue[] footstepSounds = new AudioCue[3];

   [FormerlySerializedAs("questPickupItem")] [SerializeField]
   private GameObject pickupItem;

   [SerializeField] private GameObject interactableCharacter;
   [SerializeField] private GameObject buildingMask;

   [SerializeField] private SpriteRenderer body;
   [SerializeField] private SpriteRenderer head;
   [SerializeField] private SpriteRenderer top;
   [SerializeField] private SpriteRenderer legs;
   [SerializeField] private SpriteRenderer back;
   [SerializeField] private SpriteRenderer mainHand;
   [SerializeField] private SpriteRenderer offHand;

   [SerializeField] private Image bodyInventory;
   [SerializeField] private Image headInventory;
   [SerializeField] private Image topInventory;
   [SerializeField] private Image legsInventory;
   [SerializeField] private Image backInventory;
   public ContactFilter2D cf;

   private Animation _anim;

   private Vector2 _turnBasedDir;

   public GameObject torch;

   private bool _isSkipping = false;

   private void Start()
   {
      _rb = GetComponent<Rigidbody2D>();
      _anim = GetComponent<Animation>();
      CheckSprites();
      InvokeRepeating(nameof(PlayFootstep), 1, .2f);
      InvokeRepeating(nameof(NextLine), 0, .1f);
   }

   private void NextLine()
   {
      if (_isSkipping)
      {
         Interact();
      }
   }

   private void Update()
   {
      Cursor.SetCursor(Input.GetMouseButton(0) ? cursorClosed : cursorNormal, Vector2.zero, CursorMode.Auto);

      if (MainDialogueManager.Instance.IsTalking && Input.GetKey(KeyCode.LeftControl))
         _isSkipping = true;
      else
         _isSkipping = false;

      foreach (InventoryObjectScript inventoryObjectScript in _inventoryObjects)
      {
         inventoryObjectScript.PlayerExit();
      }

      io?.PlayerEnter();

      torch.SetActive(InventoryManagerScript.instance.equippedItems[3]?.index == 5 ||
                      InventoryManagerScript.instance.equippedItems[4]?.index == 5);

      if (!MainDialogueManager.Instance.IsTalking && !MainDialogueManager.Instance.isChoosing &&
          !InventoryManagerScript.instance.inventoryMenu.activeSelf && !CGManager.IsInCG)
      {
         _horizontal = 0;
         _vertical = 0;

         if (Input.GetKey(KeyCode.A))
            _horizontal = -1;
         if (Input.GetKey(KeyCode.D))
            _horizontal = 1;
         if (Input.GetKey(KeyCode.S))
            _vertical = -1;
         if (Input.GetKey(KeyCode.W))
            _vertical = 1;

         //GetComponent<CircleCollider2D>().radius = .5f;
      }
      else
      {
         _horizontal = 0;
         _vertical = 0;
      }

      if (Mathf.Abs(_rb.velocity.x) <= .2f && Mathf.Abs(_rb.velocity.y) <= .2f)
      {
         isMoving = false;
      }
      else
      {
         isMoving = true;
      }

      RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(_horizontal, _vertical), 1);
      Debug.DrawLine(transform.position, (Vector2)transform.position + new Vector2(_horizontal, _vertical) * 1,
         Color.magenta);
      foreach (RaycastHit2D hit in hits)
      {
         // if (hit.collider.gameObject.GetComponent<PathfindingBlocker>() != null)
         //    Debug.Log("yes");
      }

      _anim.Play(isMoving ? "PlayerWalk" : "PlayerIdle");

      if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) ||
           Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !CGManager.IsInCG)
         Interact();

      CheckSprites();

      if (Input.GetMouseButtonDown(0))
      {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
         foreach (RaycastHit2D hit in hits)
         {
            if (hit.collider.gameObject.CompareTag("Interact"))
            {
               hit.collider.gameObject.GetComponent<InteractScript>().AnimalBurn();
            }
         }
      }
   }

   private void CheckSprites()
   {
      bodyInventory.sprite = bodySprites.sprites[0];
      body.sprite = _vertical > 0 ? bodySprites.sprites[1] : bodySprites.sprites[0];
      headInventory.sprite = InventoryManagerScript.instance.equippedItems[0] != null
         ? InventoryManagerScript.instance.equippedItems[0].clothes.sprites[0]
         : hairSprites.sprites[0];
      head.sprite = InventoryManagerScript.instance.equippedItems[0] != null
         ? (_vertical > 0
            ? InventoryManagerScript.instance.equippedItems[0].clothes.sprites[1]
            : InventoryManagerScript.instance.equippedItems[0].clothes.sprites[0])
         : (_vertical > 0 ? hairSprites.sprites[1] : hairSprites.sprites[0]);
      head.color = InventoryManagerScript.instance.equippedItems[0] == null ? hairMultiplyColor : Color.white;
      topInventory.sprite = InventoryManagerScript.instance.equippedItems[1]?.clothes.sprites[0];
      top.sprite = _vertical > 0
         ? InventoryManagerScript.instance.equippedItems[1]?.clothes.sprites[1]
         : InventoryManagerScript.instance.equippedItems[1]?.clothes.sprites[0];
      legsInventory.sprite = InventoryManagerScript.instance.equippedItems[2]?.clothes.sprites[0];
      legs.sprite = _vertical > 0
         ? InventoryManagerScript.instance.equippedItems[2]?.clothes.sprites[1]
         : InventoryManagerScript.instance.equippedItems[2]?.clothes.sprites[0];
      backInventory.sprite = InventoryManagerScript.instance.equippedItems[6]?.clothes.sprites[0];
      back.sprite = _vertical > 0
         ? InventoryManagerScript.instance.equippedItems[6]?.clothes.sprites[1]
         : InventoryManagerScript.instance.equippedItems[6]?.clothes.sprites[0];
      if (InventoryManagerScript.instance.equippedItems[3]?.clothes != null)
         mainHand.sprite = InventoryManagerScript.instance.equippedItems[3]?.clothes.sprites[0] != null
            ? InventoryManagerScript.instance.equippedItems[3]?.clothes.sprites[0]
            : null;
      else
         mainHand.sprite = null;
      if (InventoryManagerScript.instance.equippedItems[4]?.clothes != null)
         offHand.sprite = InventoryManagerScript.instance.equippedItems[4]?.clothes.sprites[0] != null
            ? InventoryManagerScript.instance.equippedItems[4]?.clothes.sprites[0]
            : null;
      else
         offHand.sprite = null;
      mainHand.sortingOrder = _vertical > 0 ? 1 : -1;
      offHand.sortingOrder = _vertical > 0 ? 1 : -1;

      topInventory.color = topInventory.sprite == null ? new Color(255, 255, 255, 0) : new Color(255, 255, 255, 255);
      legsInventory.color = legsInventory.sprite == null ? new Color(255, 255, 255, 0) : new Color(255, 255, 255, 255);
      backInventory.color = backInventory.sprite == null ? new Color(255, 255, 255, 0) : new Color(255, 255, 255, 255);
      headInventory.color = headInventory.sprite == null ? new Color(255, 255, 255, 0) : new Color(255, 255, 255, 255);

   }

   private void CheckSpritesMouse()
   {
      body.sprite = GetMousePos().y > transform.position.y ? bodySprites.sprites[1] : bodySprites.sprites[0];
      head.sprite = InventoryManagerScript.instance.equippedItems[0] != null
         ? (GetMousePos().y > transform.position.y
            ? InventoryManagerScript.instance.equippedItems[0].clothes.sprites[1]
            : InventoryManagerScript.instance.equippedItems[0].clothes.sprites[0])
         : (GetMousePos().y > transform.position.y ? hairSprites.sprites[1] : hairSprites.sprites[0]);
      top.sprite = GetMousePos().y > transform.position.y
         ? InventoryManagerScript.instance.equippedItems[1]?.clothes.sprites[1]
         : InventoryManagerScript.instance.equippedItems[1]?.clothes.sprites[0];
      legs.sprite = GetMousePos().y > transform.position.y
         ? InventoryManagerScript.instance.equippedItems[2]?.clothes.sprites[1]
         : InventoryManagerScript.instance.equippedItems[2]?.clothes.sprites[0];
      back.sprite = GetMousePos().y > transform.position.y
         ? InventoryManagerScript.instance.equippedItems[6]?.clothes.sprites[1]
         : InventoryManagerScript.instance.equippedItems[6]?.clothes.sprites[0];
      mainHand.sortingOrder = GetMousePos().y > transform.position.y ? 1 : -1;
   }

   private Vector2 GetMousePos()
   {
      return Camera.main.ScreenToWorldPoint(Input.mousePosition);
   }

   public void Interact()
   {
      //Debug.Log("Interacted");

      if (MainDialogueManager.Instance.talkingToSelf)
      {
         _rb.AddForce(MainDialogueManager.Instance.NextSelfTalkLine() * 10, ForceMode2D.Impulse);
         return;
      }

      if (pickupItem != null)
      {
         if (pickupItem.GetComponent<ItemScript>().canPickup)
         {
            //MainDialogueManager.Instance.ContinueQuest(pickupItem.GetComponent<QuestItemPickupScript>().quest);
            InventoryManagerScript.instance.AddItem(pickupItem.GetComponent<ItemScript>().item);
            Destroy(pickupItem);
         }
      }

      if (MainDialogueManager.Instance.IsTalking)
         MainDialogueManager.Instance.NextLine();

      if (io != null)
      {
         io.OpenInventory();
         io.OpenInventory();
      }
   }

   private bool _didPlayFootstepSound;

   private void PlayFootstep()
   {
      _didPlayFootstepSound = !_didPlayFootstepSound;
      if (!isMoving || _didPlayFootstepSound) return;

      AudioCue cue = isInBuilding ? footstepSounds[1] : footstepSounds[0];
      MainAudioManager.instance.SpawnAudio(cue.GetSound(), transform.position, cue.volume);

   }

   public InventoryObjectScript io = null;

   private void FixedUpdate()
   {
      if (_inventoryObjects.Count > 1)
      {
         io = _inventoryObjects[0];
         foreach (InventoryObjectScript inventoryObjectScript in _inventoryObjects)
         {
            if (io == inventoryObjectScript) continue;
            if (Vector2.Distance(transform.position, io.gameObject.transform.position) >=
                Vector2.Distance(transform.position, inventoryObjectScript.gameObject.transform.position))
            {
               io = inventoryObjectScript;
            }

         }
      }
      else if (_inventoryObjects.Count == 1)
      {
         io = _inventoryObjects[0];
         _inventoryObjects[0].PlayerEnter();
      }
      else io = null;

      if (canMove)
         _rb.AddForce(new Vector2(_horizontal * runSpeed, _vertical * runSpeed));
   }

   private static bool Includes<T>(IReadOnlyCollection<T> firstList, IReadOnlyCollection<T> secondList)
   {
      return firstList.Count <= secondList.Count && firstList.Select(secondList.Contains).All(isPresent => isPresent);
   }

   public void Teleport(Vector2 pos, bool force = false)
   {
      if (force)
         transform.position = pos;
      else
         _rb.MovePosition(pos);
   }

   public List<InventoryObjectScript> _inventoryObjects = new();

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Item"))
         pickupItem = other.gameObject;
      if (other.CompareTag("InteractableCharacter") || other.GetComponent<DialogueSystemCharacterScript>())
      {
         interactableCharacter = other.gameObject;
         interactableCharacter.GetComponent<DialogueSystemCharacterScript>().CheckActivity();
      }

      if (other.CompareTag("Building"))
         buildingMask.SetActive(false);
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("Item"))
         pickupItem = null;
      if (other.CompareTag("InteractableCharacter") && interactableCharacter)
      {
         interactableCharacter.GetComponent<DialogueSystemCharacterScript>().CheckActivity();
         interactableCharacter = null;
      }

      if (other.CompareTag("Building"))
         buildingMask.SetActive(true);
   }
}
