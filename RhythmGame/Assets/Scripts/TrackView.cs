using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RhythmGame
{
    /// <summary>
    /// Track View Component.
    /// </summary>
    [AddComponentMenu("UnityCoach/Beats/TrackView")]
    [RequireComponent(typeof(VerticalLayoutGroup))]
    [RequireComponent(typeof(ContentSizeFitter))]
    [RequireComponent(typeof(RectTransform))]
    public class TrackView : MonoBehaviour
    {
        public enum Trigger
        {
            Missed,
            Right,
            Wrong
        }

        //      [SerializeField] Track _track;

        [Header("Beats' Display")]
        [SerializeField] RectTransform _left;
        [SerializeField] RectTransform _right;
        [SerializeField] RectTransform _up;
        [SerializeField] RectTransform _down;
        [SerializeField] RectTransform _empty;

        RectTransform _rTransform; 
        List<Image> _beatViews;

        Vector2 _position; 
        float _beatViewSize; 
        float _spacing; 

        /// <summary>
        /// The scrolling position of the TrackView.
        /// </summary>
        public float position
        {
            get { return _position.y; }
            set
            {
                if (value != _position.y)
                {
                    _position.y = value;
                    _rTransform.anchoredPosition = _position;
                }
            }
        }

        /// <summary>
        /// Initialises the TrackView with the specified Track.
        /// </summary>
        /// <param name="track">The Track to initialise the TrackView with.</param>
        public void Init(Track track)
        {
            _rTransform = (RectTransform)transform;
            _position = _rTransform.anchoredPosition;

            _beatViewSize = _empty.rect.height;
            _spacing = GetComponent<VerticalLayoutGroup>().spacing;

            _beatViews = new List<Image>();

            foreach (int b in track.beats)
            {
                GameObject g;
                switch (b)
                {
                    case 0:
                        g = _left.gameObject;
                        break;
                    case 1:
                        g = _down.gameObject;
                        break;
                    case 2:
                        g = _up.gameObject;
                        break;
                    case 3:
                        g = _right.gameObject;
                        break;
                    default:
                        g = _empty.gameObject;
                        break;
                }
                Image view = GameObject.Instantiate(g, transform).GetComponent<Image>();
                view.transform.SetAsFirstSibling();

                _beatViews.Add(view);
            }
        }

        void Start()
        {
            Init(GamePlayController.Instance.track);
        }

        void Update()
        {
            position -= (_beatViewSize + _spacing) * Time.deltaTime * GamePlayController.Instance.secondsPerBeat;
        }

        /// <summary>
        /// Triggers beat view update.
        /// </summary>
        /// <param name="index">Index of the Beat View to update.</param>
        /// <param name="trigger">Update Trigger Type.</param>
        public void TriggerBeatView(int index, Trigger trigger)
        {
            switch (trigger)
            {
                case Trigger.Missed:
                    _beatViews[index].color = Color.gray;
                    break;
                case Trigger.Right:
                    _beatViews[index].color = Color.yellow;
                    break;
                case Trigger.Wrong:
                    _beatViews[index].color = Color.cyan;
                    break;
            }
        }
    }
}