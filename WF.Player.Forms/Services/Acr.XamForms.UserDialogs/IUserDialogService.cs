using System;

namespace Acr.XamForms.UserDialogs {

    public interface IUserDialogService {

        void Alert(AlertConfig config);
        void ActionSheet(ActionSheetConfig config);
        
        void Confirm(ConfirmConfig config);
        void Prompt(PromptConfig config);
    }
}