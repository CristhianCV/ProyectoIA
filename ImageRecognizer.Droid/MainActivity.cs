using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Widget;

using System;
using System.IO;
using Environment = Android.OS.Environment;
using Path = System.IO.Path;
using Uri = Android.Net.Uri;




namespace ImageRecognizer.Droid
{
    [Activity(Label = "Reconocimiento de imágenes", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const int CAMERA_CAPTURE_IMAGE_REQUEST_CODE = 100;
        private const string IMAGE_DIRECTORY_NAME = "Reconocimiento de imágenes";
        private Uri fileUri;

        private Button btnCapturePicture;
        private Button btnGallery;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            btnCapturePicture = FindViewById<Button>(Resource.Id.btnCapturePicture);
            btnGallery = FindViewById<Button>(Resource.Id.btnGallery);
            Plugin.TextToSpeech.CrossTextToSpeech.Current.Init();
            btnCapturePicture.Click += (s, args) =>
            {
                TakePicture();
            };
            btnGallery.Click += (s, args) =>
            {
                OpenGallery();
            };
        }

        #region HelperMethods

        private void OpenGallery() {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }
        private void TakePicture()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            fileUri = GetOutputMediaFile(
                IMAGE_DIRECTORY_NAME,
                String.Empty);
            intent.PutExtra(MediaStore.ExtraOutput, fileUri);
            StartActivityForResult(intent, CAMERA_CAPTURE_IMAGE_REQUEST_CODE);
        }

        //  From Github:
        //  https://github.com/xamurais/Xamarin.android/tree/master/CamaraFoto
        private Uri GetOutputMediaFile(string subdir, string name)
        {
            subdir = subdir ?? String.Empty;

            //  Name the pic
            if (String.IsNullOrWhiteSpace(name))
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                name = "IMG_" + timestamp + ".jpg";
            }

            //  Get cellphone pictures directory
            string mediaType = Environment.DirectoryPictures;

            //  Get complete path
            using (Java.IO.File mediaStorageDir = new Java.IO.File(
                Environment.GetExternalStoragePublicDirectory(mediaType), subdir))
            {
                //  If the directory doesn't exist, create it
                if (!mediaStorageDir.Exists())
                {
                    if (!mediaStorageDir.Mkdirs())
                        throw new IOException("No se pudo crear el directorio");
                }

                return Uri.FromFile(new Java.IO.File(GetUniquePath(mediaStorageDir.Path, name)));
            }
        }

        private string GetUniquePath(string path, string name)
        {
            //  Apply a unique name
            string ext = Path.GetExtension(name);
            if (ext == String.Empty)
                ext = ".jpg";

            name = Path.GetFileNameWithoutExtension(name);

            string nname = name + ext;
            int i = 1;
            while (File.Exists(Path.Combine(path, nname)))
                nname = $"{name}_{i++}{ext}";

            return Path.Combine(path, nname);
        }

        #endregion HelperMethods

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == CAMERA_CAPTURE_IMAGE_REQUEST_CODE)
            {
                //  Check answer (OK or canceled)
                if (resultCode == Result.Ok)
                {
                    Intent i = new Intent(this, typeof(ResultActivity));
                    i.PutExtra("fileUri", fileUri);
                    StartActivity(i);
                }
                else if (resultCode == Result.Canceled)
                {
                    Toast.MakeText(this.ApplicationContext,
                        "La captura de la imagen fue cancelada.",
                        ToastLength.Short)
                        .Show();
                }
                else
                {
                    Toast.MakeText(this.ApplicationContext,
                        "Ups! Algo raro pasó. Inténtalo de nuevo :)!",
                        ToastLength.Short)
                        .Show();
                }
            }
            else{
                if (resultCode == Result.Ok)
                {

                    ICursor cursor = ContentResolver.Query(data.Data, null, null, null, null);
                    cursor.MoveToFirst();
                    string documentId = cursor.GetString(0);
                    documentId = documentId.Split(':')[1];
                    cursor.Close();

                    cursor = ContentResolver.Query(
                    Android.Provider.MediaStore.Images.Media.ExternalContentUri,
                    null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new[] { documentId }, null);
                    cursor.MoveToFirst();
                    string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
                    cursor.Close();

                    Intent i = new Intent(this, typeof(ResultActivity));
                    i.PutExtra("fileUri", Uri.Parse(path));
                    StartActivity(i);
                }
            }
        }
    }
}
