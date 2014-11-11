using System;


namespace Acr.XamForms.UserDialogs {

    public abstract class AbstractUserDialogService : IUserDialogService {
        
        public abstract void Alert(AlertConfig config);
        public abstract void ActionSheet(ActionSheetConfig config);
        public abstract void Confirm(ConfirmConfig config);
        public abstract void Prompt(PromptConfig config);
    }
}
