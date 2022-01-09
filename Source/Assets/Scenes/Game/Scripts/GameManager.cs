using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Game.Scripts
{
	public class GameManager : MonoBehaviour
	{
		#region Serialized fields

		[SerializeField] private TextMeshProUGUI marioCoinsUI;
		[SerializeField] private TextMeshProUGUI marioPointsUI;
		[SerializeField] private TextMeshProUGUI marioLivesUI;
		[SerializeField] private TextMeshProUGUI luigiCoinsUI;
		[SerializeField] private TextMeshProUGUI luigiPointsUI;
		[SerializeField] private TextMeshProUGUI luigiLivesUI;
		[SerializeField] private TextMeshProUGUI timeUI;
		[SerializeField] private TextMeshProUGUI message;

		#endregion

		#region Constants

		public const string GroundTag = "Ground";
		public const string MarioTag = "Mario";
		private const int MaxGameTime = 400;
		private const int GameResetDelay = 4;
		private const int InstructionsBuildIndex = 0;

		#endregion

		#region Private fields

		private static GameManager _shared;
		private Transform _cameraTransform;

		private static Vector3 _lastPos = Vector3.zero;
		private int _marioCoins = 0;
		private int _luigiCoins = 0;
		private int _marioPoints = 0;
		private int _luigiPoints = 0;
		private int _marioLives = 0;
		private int _luigiLives = 0;
		private int _timePassed = 0;
		private bool _onePlayerLeft = false;
		private float _startTime;

		#endregion

		#region Public static properties

		public static Vector3 CameraPosition
		{
			get
			{
				if (_lastPos != Vector3.zero)
					return _lastPos;
				return _shared._cameraTransform == null
					? _shared.transform.position
					: _shared._cameraTransform.position;
			}
		}

		#endregion

		#region Private static properties

		private static int MarioCoins
		{
			get => _shared._marioCoins;
			set
			{
				_shared._marioCoins = value;
				_shared.marioCoinsUI.text = value > 9 ? $" x{value}" : $" x0{value}";
			}
		}

		private static int LuigiCoins
		{
			get => _shared._luigiCoins;
			set
			{
				_shared._luigiCoins = value;
				_shared.luigiCoinsUI.text = value > 9 ? $" x{value}" : $" x0{value}";
			}
		}

		private static int MarioPoints
		{
			get => _shared._marioPoints;
			set
			{
				_shared._marioPoints = value;
				var stringValue = value.ToString();
				var pointsPrefix = "00000".Substring(stringValue.Length);
				_shared.marioPointsUI.text = pointsPrefix + stringValue;
			}
		}

		private static int LuigiPoints
		{
			get => _shared._luigiPoints;
			set
			{
				_shared._luigiPoints = value;
				var stringValue = value.ToString();
				var pointsPrefix = "00000".Substring(stringValue.Length);
				_shared.luigiPointsUI.text = pointsPrefix + stringValue;
			}
		}

		private static int MarioLives
		{
			get => _shared._marioLives;
			set
			{
				_shared._marioLives = value;
				_shared.marioLivesUI.text = value > 9 ? $" x{value}" : $" x0{value}";
			}
		}

		private static int LuigiLives
		{
			get => _shared._luigiLives;
			set
			{
				_shared._luigiLives = value;
				_shared.luigiLivesUI.text = value > 9 ? $" x{value}" : $" x0{value}";
			}
		}

		private static int TimePassed
		{
			get => _shared._timePassed;
			set
			{
				_shared._timePassed = value;
				_shared.timeUI.text = (MaxGameTime - value).ToString();
			}
		}

		#endregion

		#region Function events

		private void Awake() => InitFields();

		private void Update()
		{
			UpdateGameTime();
			CheckPlayerEscape();
		}

		private void OnDestroy() => _lastPos = transform.position;

		#endregion

		#region Public static methods

		/// <summary>
		/// Register a PowerUp collection.
		/// </summary>
		/// /// <param name="player">The registering player.</param>
		public static void PowerUpCollected(PlayerIdentifier player)
		{
			if (player == PlayerIdentifier.Mario)
				MarioPoints += 400;
			else
				LuigiPoints += 400;
		}

		/// <summary>
		/// Register a coin collection.
		/// </summary>
		/// <param name="player">The registering player.</param>
		public static void AddCoin(PlayerIdentifier player)
		{
			if (player == PlayerIdentifier.Mario)
			{
				++MarioCoins;
				MarioPoints += 200;
			}
			else
			{
				++LuigiCoins;
				LuigiPoints += 200;
			}
		}

		/// <summary>
		/// Register an enemy kill.
		/// </summary>
		/// /// <param name="player">The player registering the kill.</param>
		/// <param name="streak">The current kill streak.</param>
		public static void EnemyKilled(PlayerIdentifier player, int streak)
		{
			if (player == PlayerIdentifier.Mario)
				MarioPoints += 100 * (int) Math.Pow(2, streak);
			else
				LuigiPoints += 100 * (int) Math.Pow(2, streak);
		}

		/// <summary>
		/// Register a flagpole slide
		/// </summary>
		/// <param name="player">The sliding player.</param>
		/// <param name="height">The starting height of the slide.</param>
		public static void FlagPoleSlide(PlayerIdentifier player, float height)
		{
			if (player == PlayerIdentifier.Mario)
				MarioPoints += 100 * (1 + (int) height);
			else
				LuigiPoints += 100 * (1 + (int) height);
		}

		public static int AddLife(PlayerIdentifier player)
		{
			if (player == PlayerIdentifier.Mario)
				return ++MarioLives;
			return ++LuigiLives;
		}

		public static int RemoveLife(PlayerIdentifier player)
		{
			if (player == PlayerIdentifier.Mario)
				return --MarioLives;
			return --LuigiLives;
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Returns the players to the instructions page when pressing escape. 
		/// </summary>
		private static void CheckPlayerEscape()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				SceneManager.LoadSceneAsync(InstructionsBuildIndex);
		}

		/// <summary>
		/// Resets the game with a delay.
		/// </summary>
		/// <param name="delay">Delay in seconds.</param>
		private void ResetAfterDelay(float delay) => StartCoroutine(ResetAfterDelayCoroutine(delay));

		/// <summary>
		/// Initializes the class fields.
		/// </summary>
		private void InitFields()
		{
			_shared = this;
			var cam = Camera.main;
			_cameraTransform = cam == null ? null : cam.transform;
			_startTime = Time.time;
			TimePassed = 0;
		}

		/// <summary>
		/// Updates the game time and resets game at time end.
		/// </summary>
		private static void UpdateGameTime()
		{
			int elapsedTime = (int) (Time.time - _shared._startTime);
			if (elapsedTime > TimePassed)
				TimePassed = elapsedTime;
			if (TimePassed == MaxGameTime)
				EndGame();
		}

		#endregion

		#region Coroutines

		private static IEnumerator ResetAfterDelayCoroutine(float time)
		{
			yield return new WaitForSeconds(time);
			TimePassed = 0;
			SceneManager.LoadSceneAsync(InstructionsBuildIndex);
		}

		#endregion

		public static void RegisterDeath()
		{
			if (_shared._onePlayerLeft)
				EndGame();
			else
				_shared._onePlayerLeft = true;
		}

		public static void EndGame()
		{
			if (MarioPoints > LuigiPoints)
				_shared.message.text = "Mario wins!";
			else if (MarioPoints < LuigiPoints)
				_shared.message.text = "Luigi wins!";
			else
				_shared.message.text = "It's a draw!";
			_shared.ResetAfterDelay(GameResetDelay);
		}

		public static bool Buy1Up(PlayerIdentifier player)
		{
			switch (player)
			{
				case PlayerIdentifier.Mario when MarioCoins >= 10:
					MarioCoins -= 10;
					return true;
				case PlayerIdentifier.Luigi when LuigiCoins >= 10:
					LuigiCoins -= 10;
					return true;
				default:
					return false;
			}
		}
	}
}