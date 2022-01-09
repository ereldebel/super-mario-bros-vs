using System;
using UnityEngine;

namespace Scenes.Game.Scripts.Mario
{
	/// <summary>
	/// Controls the level clear cutscene.
	/// </summary>
	public class CutsceneController : MonoBehaviour
	{
		[SerializeField] private float slideForce = 0.1f;

		public Action LowerFlag { private get; set; } = null;

		private Animator _animator;
		private Rigidbody2D _rigidbody2D;
		private PlayerMovement _playerMovement;
		private MarioManager _marioManager;
		private Collider2D _collider2D;
		private bool _sliding = true;
		private GameObject _flag = null;
		private static readonly int GetFlag = Animator.StringToHash("Get Flag");
		private static readonly int Slide = Animator.StringToHash("Slide");

		#region Function events

		private void Awake() => InitFields();

		public void OnEnable()
		{
			_playerMovement.enabled = false;
			_collider2D.enabled = true;
			_sliding = true;
			_animator.SetBool(Slide, true);
			AudioManager.FlagpoleSlide();
		}

		private void OnDisable() => _playerMovement.enabled = true;

		private void Update()
		{
			if (_sliding && _marioManager.Grounded)
			{
				_sliding = false;
				AudioManager.StageClear();
				_animator.SetBool(Slide, false);
				_animator.SetBool(GetFlag, true);
				LowerFlag();
			}

			if (!_sliding && _flag != null && transform.position.y >= _flag.transform.position.y)
				TakeFlag();
		}

		private void FixedUpdate()
		{
			if (!_sliding) return;
			_rigidbody2D.velocity = Vector2.zero;
			var pos = _rigidbody2D.position;
			pos += Vector2.down * slideForce;
			_rigidbody2D.position = pos;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Register flag as gotten by mario.
		/// </summary>
		/// <param name="flag">The flag at the top of the pole.</param>
		public void GotFlag(GameObject flag)
		{
			_flag = flag;
			if (_sliding)
			{
				LowerFlag();
				return;
			}

			TakeFlag();
		}

		#endregion

		#region Private methods

		private void TakeFlag()
		{
			_flag.SetActive(false);
			_flag = null;
			GameManager.EndGame();
			enabled = false;
		}

		private void InitFields()
		{
			_animator = GetComponent<Animator>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_playerMovement = GetComponent<PlayerMovement>();
			_marioManager = GetComponent<MarioManager>();
			_collider2D = GetComponent<Collider2D>();
		}

		#endregion
	}
}