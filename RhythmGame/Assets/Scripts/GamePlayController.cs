using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame
{
    public class GamePlayController : MonoBehaviour
    {
        #region Variables
        [Header("Inputs")]
        [SerializeField] KeyCode _left = KeyCode.LeftArrow;
        [SerializeField] KeyCode _down = KeyCode.DownArrow;
        [SerializeField] KeyCode _up = KeyCode.UpArrow;
        [SerializeField] KeyCode _right = KeyCode.RightArrow;

        [Header("Track")]
        [SerializeField] private Track _track;
        /// <summary>
        /// The current Track.
        /// </summary>
        public Track track { get { return _track; } }

        /// <summary>
        /// number of seconds per beat.
        /// </summary>
        public float secondsPerBeat { get; private set; }

        /// <summary>
        /// number of beats per second.
        /// </summary>
        public float beatsPerSecond { get; private set; }

        /// <summary>
        /// If the player has played within the current beat
        /// </summary>
        private bool _played;

        /// <summary>
        /// Track completed
        /// </summary>
        private bool _completed;

        private TrackView _trackView; 
        private WaitForSeconds waitAndStop;
        #endregion

        #region Singleton
        private static GamePlayController _instance;
        /// <summary>
        /// Current GamePlayController instance.
        /// </summary>
        public static GamePlayController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = (GamePlayController)GameObject.FindObjectOfType(typeof(GamePlayController));

                return _instance;
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }
        #endregion

        #region MonoBehaviour Methods
        void Awake()
        {
            _instance = this;
            secondsPerBeat = track.bpm / 60f;
            beatsPerSecond = 60f / track.bpm;
            waitAndStop = new WaitForSeconds(beatsPerSecond * 2);

            _trackView = FindObjectOfType<TrackView>();
            if (!_trackView)
                Debug.LogWarning("No TrackView found in current scene");
        }

        void Start()
        {
            InvokeRepeating("NextBeat", 0f, beatsPerSecond);
        }

        void Update()
        {
            if (_played || _completed)
                return;

            if (Input.GetKeyDown(_left))
                PlayBeat(0);
            if (Input.GetKeyDown(_down))
                PlayBeat(1);
            if (Input.GetKeyDown(_up))
                PlayBeat(2);
            if (Input.GetKeyDown(_right))
                PlayBeat(3);
        }
        #endregion

        #region GamePlay
        private int _current;
        /// <summary>
        /// The current beat index.
        /// </summary>
        public int current
        {
            get { return _current; }
            private set
            {
                if (value != _current)
                {
                    _current = value;

                    if (_current == _track.beats.Count)
                    {
                        CancelInvoke("NextBeat");

                        _completed = true;

                        StartCoroutine(WaitAndStop());
                    }
                }
            }
        }

        void PlayBeat(int input)
        {
            _played = true;

            if (_track.beats[current] == -1)
            {
                Debug.Log (string.Format("{0} played untimely", input));
            }
            else if (_track.beats[current] == input)
            {
                _trackView.TriggerBeatView(current, TrackView.Trigger.Right);
            }
            else
            {
                _trackView.TriggerBeatView(current, TrackView.Trigger.Wrong);
            }
        }

        void NextBeat()
        {

            if (!_played && _track.beats[current] != -1)
            {
                _trackView.TriggerBeatView(current, TrackView.Trigger.Missed);
            }
            _played = false;

            current++;
        }

        private IEnumerator WaitAndStop()
        {
            yield return waitAndStop;
            enabled = false;
        }
        #endregion
    }
}