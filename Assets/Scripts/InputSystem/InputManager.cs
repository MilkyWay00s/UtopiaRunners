using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InputSystem.Interface;



namespace InputSystem
{
    public enum ActionCode
    {
        Tag,
        Skill,
        Slide,
        Jump,
        Option,

        Menu,
        MenuLeft,
        MenuRight,

    }
    public class InputManager : SingletonObject<InputManager>
    {
        private const float KeyListenerDelay = 0.05f;
        private const float KeyDownDelay = 1f;

        private Dictionary<ActionCode, bool> _keyDownBools = new Dictionary<ActionCode, bool>();
        private Dictionary<ActionCode, bool> _keyDownBoolsForListener = new Dictionary<ActionCode, bool>();
        private Dictionary<ActionCode, Coroutine> _keyDownCounterCoroutine = new Dictionary<ActionCode, Coroutine>();
        private Dictionary<ActionCode, bool> _keyActiveFlags = new Dictionary<ActionCode, bool>();
        private Dictionary<ActionCode, KeyCode> _keyMappings = new Dictionary<ActionCode, KeyCode>();
        private Dictionary<ActionCode, KeyCode> _altKeyMappings = new Dictionary<ActionCode, KeyCode>();


        //씬이 로드될 때마다 OnSceneUnloading 함수 실행되도록 등록.
        protected override void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneUnloading;
        }

        //씬 로드 시 초기화
        private void OnSceneUnloading(Scene arg0, LoadSceneMode arg1)
        {
            foreach (ActionCode action in Enum.GetValues(typeof(ActionCode)))
            {
                _keyDownBools[action] = false;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneUnloading;
        }

        //게임의 기본 키 설정을 정의
        public void SetDefaultKey()
        {
            _keyMappings = new Dictionary<ActionCode, KeyCode>()
            {
                { ActionCode.Jump, KeyCode.Space },
                { ActionCode.Tag, KeyCode.Q },
                { ActionCode.Skill, KeyCode.Space },
                { ActionCode.Slide, KeyCode.LeftControl },
                { ActionCode.Option, KeyCode.Space },
             


                //편집 필요
            };

            _altKeyMappings = new Dictionary<ActionCode, KeyCode>()
            {
                { ActionCode.Jump, KeyCode.UpArrow },                
                { ActionCode.Slide, KeyCode.DownArrow },

            };
        }

        public static event Action<ActionCode, InputType> OnKeyEvent;

        //특정 액션을 임시로 막을 수 있음
        public void SetKeyActive(ActionCode action, bool active)
        {
            _keyActiveFlags[action] = active;
        }

        //상호작용 키 제외하고 전체 키 활성화/비활성화 설정.
        /*public void SetActionState(bool active)
        {
            foreach (ActionCode actionCode in _keyMappings.Keys)
            {
                if (actionCode != ActionCode.Interaction)
                    SetKeyActive(actionCode, active);
            }
        }       
        불필요해보여 주석처리*/

        //키 활성 여부, 전체 매핑 조회용 getter.
        public bool GetKeyActive(ActionCode action) => _keyActiveFlags[action];
        public Dictionary<ActionCode, KeyCode> GetKeyActions() => _keyMappings;

        //키 변경
        public void SetKey(ActionCode actionCode, KeyCode newKey)
        {
            if (_keyMappings.ContainsKey(actionCode))
                _keyMappings[actionCode] = newKey;
        }

        public void SetAltKey(ActionCode actionCode, KeyCode newAltKey)
        {
            if (_altKeyMappings.ContainsKey(actionCode))
                _altKeyMappings[actionCode] = newAltKey;
            else
                _altKeyMappings.Add(actionCode, newAltKey);
        }

        //키눌림감지(한번만)
        public bool GetKeyDown(ActionCode action)
        {
            try
            {
                if (_keyActiveFlags[action] && _keyDownBools[action])
                {
                    _keyDownBools[action] = false;
                    return true;
                }
            }
            catch (KeyNotFoundException)
            {
                _keyActiveFlags[action] = true;
                _keyDownBools[action] = false;
            }
            return false;
        }


        //키홀드 감지
        public bool GetKey(ActionCode action)
        {
            try
            {
                bool main = Input.GetKey(_keyMappings[action]);
                bool alt = _altKeyMappings.ContainsKey(action) && Input.GetKey(_altKeyMappings[action]);
                return (main || alt) && _keyActiveFlags[action];

            }
            catch (KeyNotFoundException)
            {
                SetDefaultKey();
                return false;
            }
        }


        //키가 눌렸을 때 일정 시간 후 자동 초기화
        private IEnumerator KeyDownCounter(ActionCode action)
        {
            yield return new WaitForSeconds(KeyDownDelay);
            _keyDownBools[action] = false;
            _keyDownCounterCoroutine[action] = null;
        }


        private void Update()
        {
            foreach (ActionCode action in _keyMappings.Keys)
            {
                try
                {
                    if (_keyActiveFlags[action])
                    {
                        bool mainDown = Input.GetKeyDown(_keyMappings[action]);
                        bool altDown = _altKeyMappings.ContainsKey(action) && Input.GetKeyDown(_altKeyMappings[action]);

                        if (mainDown || altDown)
                        {
                            _keyDownBools[action] = true;
                            _keyDownBoolsForListener[action] = true;
                            Coroutine tempCoroutine = _keyDownCounterCoroutine[action];
                            if (tempCoroutine != null) StopCoroutine(tempCoroutine);
                            _keyDownCounterCoroutine[action] = StartCoroutine(KeyDownCounter(action));
                        }
                    }
                }
                catch (KeyNotFoundException)
                {
                    SetDefaultKey();
                    _keyDownCounterCoroutine[action] = null;
                    _keyDownBools[action] = false;
                    _keyActiveFlags[action] = true;
                }
            }
        }

        //처음 시작할 때 모든 액션 상태 초기화
        private void InitKeyDownDictionary()
        {
            foreach (ActionCode action in Enum.GetValues(typeof(ActionCode)))
            {
                _keyDownBools[action] = false;
                _keyDownCounterCoroutine[action] = null;
                _keyActiveFlags[action] = true;
                _keyDownBoolsForListener[action] = false;
            }
        }


        //입력 이벤트 감지 코루틴(외부 리스너에 이벤트 전달)
        private IEnumerator CallListenersCoroutine()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(KeyListenerDelay);
                foreach (ActionCode action in _keyMappings.Keys)
                {
                    if (_keyActiveFlags[action])
                    {
                        bool mainPress = Input.GetKey(_keyMappings[action]);
                        bool altPress = _altKeyMappings.ContainsKey(action) && Input.GetKey(_altKeyMappings[action]);

                        bool mainUp = Input.GetKeyUp(_keyMappings[action]);
                        bool altUp = _altKeyMappings.ContainsKey(action) && Input.GetKeyUp(_altKeyMappings[action]);

                        if (_keyDownBoolsForListener[action])
                        {
                            _keyDownBoolsForListener[action] = false;
                            OnKeyEvent?.Invoke(action, InputType.Down);
                        }
                        else if (mainPress || altPress)
                        {
                            OnKeyEvent?.Invoke(action, InputType.Press);
                        }
                        else if (mainUp || altUp)
                        {
                            OnKeyEvent?.Invoke(action, InputType.Up);
                        }
                    }
                }
            }
        }


        private void Start()
        {
            SetDefaultKey();
            InitKeyDownDictionary();
            StartCoroutine(CallListenersCoroutine());
        }
    }
}

