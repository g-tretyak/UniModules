﻿using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Drawers;
using SubModules.Scripts.UniStateMachine.NodeEditor;
using UniEditorTools;
using UniNodeSystemEditor;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    using UniGreenModules.UniNodeSystem.Runtime.BaseNodes;
    using UniGreenModules.UniNodeSystem.Runtime.Runtime;

    [CustomNodeEditorAttribute(typeof(GraphInputNode))]
    public class UniGraphInputNodeEditor : UniNodeEditor
    {
        protected override List<INodeEditorDrawer> InitializedBodyDrawers()
        {
            var drawers = base.InitializedBodyDrawers();
            drawers.Add(new RenameFiedDrawer());
            drawers.Add(new ButtonActionBodyDrawer("add out", () =>
            {
                if(NodeEditorWindow.current == null)
                    return;
                
                var nodeName = target.GetName().
                    Replace(UniBaseNode.InputTriggerPrefix, "");
                NodeEditorWindow.current.CreateNode(typeof(GraphOuputNode),nodeName,
                    target.position + new Vector2(30,30));
            }));

            return drawers;
        }
    }
    
}
