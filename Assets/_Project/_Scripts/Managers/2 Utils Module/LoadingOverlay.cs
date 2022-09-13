using System;
using UnityEngine;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.Core;

namespace ArcadianLab.SimFramework.Utils
{
    public class LoadingOverlay : Object_
    {
        public static LoadingOverlay Instance;
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _loadingImage;
        private const float loadingRotationSpeed = 100f;

        public override void Init()
        {
            Assert.IsNotNull(_panel);
            Instance = this;
        }

        private void Start() => Show(false);

        private void OnEnable()
        {
            /*SignIn.StateChanged += SignIn_;
            SignUp.StateChanged += SignUp_;
            ForgetPassword.StateChanged += ForgetPassword_;*/
        }

        private void OnDisable()
        {
            /*SignIn.StateChanged -= SignIn_;
            SignUp.StateChanged -= SignUp_;
            ForgetPassword.StateChanged -= ForgetPassword_;*/
        }

        public void Show(bool value)
        {
            if (_panel)
            {
                _panel.gameObject.SetActive(value);
                _loadingImage.transform.rotation = Quaternion.identity;
            }
        }

        void Update()
        {
            if (_panel.activeInHierarchy) _loadingImage.transform.Rotate(0, 0, -loadingRotationSpeed * Time.deltaTime);
        }

        private void SignIn_(Enum type)
        {
            /*SignInState state = (SignInState)type;
            if (state == SignInState.Authenticating) Show(true);
            else if (state == SignInState.AuthenticationSuccessful ||
                state == SignInState.AuthenticationFailed) Show(false);*/

        }

        private void SignUp_(Enum type)
        {
            /*SignUpState state = (SignUpState)type;
            if (state == SignUpState.SigningUp || 
                state == SignUpState.VerifyingEmail || 
                state == SignUpState.ResendingCode) Show(true);
            else Show(false);*/
        }

        private void ForgetPassword_(Enum type)
        {
            /*ForgetPasswordState state = (ForgetPasswordState)type;
            if (state == ForgetPasswordState.ForgetPasswordSendingRequest ||
                state == ForgetPasswordState.ResetPasswordSendingRequest) Show(true);
            else Show(false);*/
        }
    }
}
