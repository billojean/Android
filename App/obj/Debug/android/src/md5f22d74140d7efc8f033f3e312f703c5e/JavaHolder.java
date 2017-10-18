package md5f22d74140d7efc8f033f3e312f703c5e;


public class JavaHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("App.JavaHolder, App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", JavaHolder.class, __md_methods);
	}


	public JavaHolder ()
	{
		super ();
		if (getClass () == JavaHolder.class)
			mono.android.TypeManager.Activate ("App.JavaHolder, App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
