using System.Collections;
using UnityEngine;


#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool shoot;
		public bool ads;
		public bool reload;
		public bool switching;
		public bool firstWeapon;
		public bool secondWeapon;
		public bool thirdWeapon;

		public WeaponHandler handler;

		private Weapon weapon;

		void Start()
		{
			weapon = handler.CurrentWeapon();
			switching = true;
		}

		void Update()
		{
			if (handler.WeaponChanged() && !shoot)
			{
				weapon = handler.CurrentWeapon();
			}
		}

		Coroutine firing;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnShoot(InputValue value) 
		{	
		    Gun gun = weapon.GetComponent<Gun>();

		    if (gun != null) 
			{
				if (value.isPressed && gun.automatic) 
				{
					switching = false;
					StartFiring();
				} else if (!value.isPressed && gun.automatic) 
				{
					StopFiring();
					switching = true;
				}
			}
			
			shoot = value.isPressed;
		}

		public void OnADS(InputValue value)
		{
			ads = value.isPressed;
		}

		public void StartFiring() 
		{
		    Gun gun = weapon.gameObject.GetComponent<Gun>();

			if (gun != null)
			{
				firing = StartCoroutine(gun.RapidFire());
			}
		}

		public void StopFiring() 
		{
			if (firing != null) 
			{
				StopCoroutine(firing);
			}
		}

		public void OnReload(InputValue value) 
		{
		    Gun gun = weapon.gameObject.GetComponent<Gun>();

			if (value.isPressed && switching && gun != null)
			{
				StartCoroutine(gun.Reload());
			}
		}

		public void OnFirstWeapon(InputValue value) 
		{
			if (switching && value.isPressed) 
			{
				handler.SelectWeapon(0);
			}
		}

		public void OnSecondWeapon(InputValue value) 
		{
			if (switching && value.isPressed) 
			{
				handler.SelectWeapon(1);
			}
		}

		public void OnThirdWeapon(InputValue value) 
		{
			if (switching && value.isPressed) 
			{
				handler.SelectWeapon(2);
			}
		}
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}