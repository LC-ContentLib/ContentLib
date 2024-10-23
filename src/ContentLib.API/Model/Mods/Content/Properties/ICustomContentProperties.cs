namespace ContentLib.API.Model.Mods
{
    /// <summary>
    /// Interface representing the general functionality of the properties of Content present within a Mod. 
    /// The intent of this interface is to facilitate implementation onto Scriptable Objects that can be 
    /// populated within the Unity Editor. This allows for the creation of asset bundles that can then 
    /// be directly linked to Content from a mod.
    /// 
    /// For example, Enemy Properties could be attached to an <see cref="IEnemy"/> implementation, 
    /// provided that the properties match the class path of the <see cref="IEnemy"/> implementation.
    /// </summary>
    public interface ICustomContentProperties
    {
        /// <summary>
        /// Gets or sets the full class path of the content implementation.
        /// This property is used to establish a direct link between the properties and 
        /// the corresponding content in the mod.
        /// </summary>
        string ClassPath { get;}
    }
}