﻿using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMarkupLanguage.TypeHandlers
{
    [ComponentHandler(typeof(BSMLScrollViewContent))]
    public class ScrollViewHandler : TypeHandler
    {
        public override Dictionary<string, string[]> Props { get; } = new Dictionary<string, string[]>
        {
            { "id", new[]{ "id" } },
            { "maskOverflow", new[] { "mask-overflow" } }
        };

        public override void HandleType(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
        {
            BSMLScrollViewContent content = componentType.component as BSMLScrollViewContent;
            BSMLScrollViewElement scrollView = content.ScrollView;

            if (componentType.data.TryGetValue("id", out string id))
            {
                parserParams.AddEvent(id + "#PageUp", scrollView.PageUpButtonPressed);
                parserParams.AddEvent(id + "#PageDown", scrollView.PageDownButtonPressed);

                scrollView.PageUpButton = parserParams.GetObjectsWithTag("PageUpFor:" + id)
                    .Select(o => o.GetComponent<Button>())
                    .FirstOrDefault(b => b != null);

                scrollView.PageDownButton = parserParams.GetObjectsWithTag("PageDownFor:" + id)
                    .Select(o => o.GetComponent<Button>())
                    .FirstOrDefault(b => b != null);
            }

            if (componentType.data.TryGetValue("maskOverflow", out string value))
            {
                scrollView.MaskOverflow = bool.TryParse(value, out bool bval) ? bval : true;
            }
        }

        public override void HandleTypeAfterChildren(BSMLParser.ComponentTypeWithData componentType, BSMLParserParams parserParams)
        {
            BSMLScrollViewContent content = componentType.component as BSMLScrollViewContent;
            BSMLScrollViewElement scrollView = content.ScrollView;

            foreach (GameObject go in parserParams.GetObjectsWithTag("ScrollFocus"))
            {
                go.AddComponent<ItemForFocussedScrolling>();
            }

            scrollView.Setup();
            scrollView.RefreshButtonsInteractibility();
            scrollView.ScrollAt(0, false);
        }
    }
}
