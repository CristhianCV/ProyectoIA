package md5468f01771cb45012241f4e3a5450ac03;


public class ResultActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("ImageRecognizer.Droid.ResultActivity, ImageRecognizer.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ResultActivity.class, __md_methods);
	}


	public ResultActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ResultActivity.class)
			mono.android.TypeManager.Activate ("ImageRecognizer.Droid.ResultActivity, ImageRecognizer.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
