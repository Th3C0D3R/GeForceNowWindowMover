﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeForceNowWindowMover.Froms {
    using System;
    
    
    /// <summary>
    /// Eine streng typisierte Resourcenklasse, zum Suchen nach lokalisierten Zeichenfolgen und entsprechenden Formatfunktionen.
    /// </summary>
    // Die Klasse wird automatisch von StronglyTypedResourceBuilderEx mittels dem ResXFileCodeGeneratorEx Werkzeug generiert.
    // Zum Hinzufügen oder Löschen eines Members, ändern Sie die .ResX-Datei. Danach führen Sie das ResXFileCodeGeneratorEx Werkzeug aus oder Kompilieren Ihr VS.NET Projekt.
    // Copyright (c) Dmytro Kryvko 2006-2022 (http://dmytro.kryvko.googlepages.com/)
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DMKSoftware.CodeGenerators.Tools.StronglyTypedResourceBuilderEx", "2.6.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
#if !SILVERLIGHT
    [global::System.Reflection.ObfuscationAttribute(Exclude=true, ApplyToMembers=true)]
#endif
    [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public partial class WrapperForm {
        
        private static global::System.Resources.ResourceManager _resourceManager;
        
        private static object _internalSyncObject;
        
        private static global::System.Globalization.CultureInfo _resourceCulture;
        
        /// <summary>
        /// Initalisiert ein WrapperForm-Objekt.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public WrapperForm() {
        }
        
        /// <summary>
        /// Thread safe lock Objekt, die von dieser Klasse verwendet wird.
        /// </summary>
        public static object InternalSyncObject {
            get {
                if (object.ReferenceEquals(_internalSyncObject, null)) {
                    global::System.Threading.Interlocked.CompareExchange(ref _internalSyncObject, new object(), null);
                }
                return _internalSyncObject;
            }
        }
        
        /// <summary>
        /// Gibt einen gespeicherte Instanz des ResourceManager zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(_resourceManager, null)) {
                    global::System.Threading.Monitor.Enter(InternalSyncObject);
                    try {
                        if (object.ReferenceEquals(_resourceManager, null)) {
                            global::System.Threading.Interlocked.Exchange(ref _resourceManager, new global::System.Resources.ResourceManager("GeForceNowWindowMover.Froms.WrapperForm", typeof(WrapperForm).Assembly));
                        }
                    }
                    finally {
                        global::System.Threading.Monitor.Exit(InternalSyncObject);
                    }
                }
                return _resourceManager;
            }
        }
        
        /// <summary>
        /// Überschreibt die aktuelle CurrentUICulture des Threads für Alles
        /// Resource durchsucht die streng typisierte Resourcenklasse.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return _resourceCulture;
            }
            set {
                _resourceCulture = value;
            }
        }
        
        /// <summary>
        /// Sucht eine Resource 'pnlInner.Anchor'.
        /// </summary>
        public static System.Windows.Forms.AnchorStyles pnlInner_Anchor {
            get {
                return ((System.Windows.Forms.AnchorStyles)(ResourceManager.GetObject(WrapperForm.ResourceNames.pnlInner_Anchor, _resourceCulture)));
            }
        }
        
        /// <summary>
        /// Sucht eine Resource 'pnlInner.Location'.
        /// </summary>
        public static System.Drawing.Point pnlInner_Location {
            get {
                return ((System.Drawing.Point)(ResourceManager.GetObject(WrapperForm.ResourceNames.pnlInner_Location, _resourceCulture)));
            }
        }
        
        /// <summary>
        /// Sucht eine Resource 'pnlInner.Margin'.
        /// </summary>
        public static System.Windows.Forms.Padding pnlInner_Margin {
            get {
                return ((System.Windows.Forms.Padding)(ResourceManager.GetObject(WrapperForm.ResourceNames.pnlInner_Margin, _resourceCulture)));
            }
        }
        
        /// <summary>
        /// Sucht eine Resource 'pnlInner.Size'.
        /// </summary>
        public static System.Drawing.Size pnlInner_Size {
            get {
                return ((System.Drawing.Size)(ResourceManager.GetObject(WrapperForm.ResourceNames.pnlInner_Size, _resourceCulture)));
            }
        }
        
        /// <summary>
        /// Sucht eine Resource 'pnlInner.TabIndex'.
        /// </summary>
        public static int pnlInner_TabIndex {
            get {
                return ((int)(ResourceManager.GetObject(WrapperForm.ResourceNames.pnlInner_TabIndex, _resourceCulture)));
            }
        }
        
        /// <summary>
        /// Liste aller Resourcenbezeichnungen als konstante String Felder
        /// </summary>
        internal class ResourceNames {
            
            /// <summary>
            /// Speichert die Resourcenbezeichnung 'pnlInner.Anchor'.
            /// </summary>
            public const string pnlInner_Anchor = "pnlInner.Anchor";
            
            /// <summary>
            /// Speichert die Resourcenbezeichnung 'pnlInner.Location'.
            /// </summary>
            public const string pnlInner_Location = "pnlInner.Location";
            
            /// <summary>
            /// Speichert die Resourcenbezeichnung 'pnlInner.Margin'.
            /// </summary>
            public const string pnlInner_Margin = "pnlInner.Margin";
            
            /// <summary>
            /// Speichert die Resourcenbezeichnung 'pnlInner.Size'.
            /// </summary>
            public const string pnlInner_Size = "pnlInner.Size";
            
            /// <summary>
            /// Speichert die Resourcenbezeichnung 'pnlInner.TabIndex'.
            /// </summary>
            public const string pnlInner_TabIndex = "pnlInner.TabIndex";
        }
    }
}
