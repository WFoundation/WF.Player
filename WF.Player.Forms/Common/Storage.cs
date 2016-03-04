using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WF.Player.Common
{
    public static class Storage
    {
        private static Lazy<StorageImpl> current = new Lazy<StorageImpl>(() => new StorageImpl(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        public static StorageImpl Current
        {
            get
            {
                return current.Value;
            }
        }
    }

    public class StorageImpl
    {
        public async void RemoveFile(string filename)
        {
            if (await FileExists(filename))
            {
                var file = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(filename);
                if (file != null)
                {
                    await file.DeleteAsync();
                }
            }
        }

        public string GetFullnameForSavegame(string filename)
        {
            return Path.Combine(App.PathForSavegames, filename);
        }

        public string GetFullnameForCartridge(string filename)
        {
            return Path.Combine(App.PathForCartridges, filename);
        }

        public async Task<bool> FileExists(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }
            var result = await PCLStorage.FileSystem.Current.LocalStorage.CheckExistsAsync(filename);
            return result == PCLStorage.ExistenceCheckResult.FileExists;
        }

        public async Task<bool> FileExistsAsync(string filename)
        {
            var result = await PCLStorage.FileSystem.Current.LocalStorage.CheckExistsAsync(filename);
            return result == PCLStorage.ExistenceCheckResult.FileExists;
        }

        public async Task<Stream> GetStreamForReading(string filename)
        {
            var found = await FileExists(filename);
            if (found)
            {
                    var file = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(filename);
                    var stream = await file.OpenAsync(PCLStorage.FileAccess.Read);

                    return stream;
            }
            else
            {
                return null;
            }
        }

        public async Task<Stream> GetStreamForWriting(string filename, bool append = false)
        {
            var file = await PCLStorage.FileSystem.Current.LocalStorage.CreateFileAsync(filename, append ? PCLStorage.CreationCollisionOption.OpenIfExists : PCLStorage.CreationCollisionOption.ReplaceExisting);
            var stream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite);

            return stream;
        }
    }
}
