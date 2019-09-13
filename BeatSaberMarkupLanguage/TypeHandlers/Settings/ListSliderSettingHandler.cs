﻿using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatSaberMarkupLanguage.TypeHandlers.Settings
{
    [ComponentHandler(typeof(ListSliderSetting))]
    public class ListSliderSettingHandler : TypeHandler
    {
        public override Dictionary<string, string[]> Props => new Dictionary<string, string[]>()
        {
            { "text", new[]{ "text" } },
            { "onChange", new[]{ "on-change"} },
            { "value", new[]{ "value"} },
            { "setEvent", new[]{ "set-event"} },
            { "getEvent", new[]{ "get-event"} },
            { "options", new[]{ "options", "choices" } },
            { "applyOnChange", new[] { "apply-on-change" } },
            { "formatter", new[] { "formatter" } }
        };

        public override void HandleType(Component obj, Dictionary<string, string> data, BSMLParserParams parserParams)
        {
            ListSliderSetting listSetting = obj as ListSliderSetting;

            if (data.TryGetValue("text", out string text))
                listSetting.LabelText = text;

            if (data.TryGetValue("formatter", out string formatter))
                listSetting.formatter = parserParams.actions[formatter];

            if (data.TryGetValue("applyOnChange", out string applyOnChange))
                listSetting.updateOnChange = Parse.Bool(applyOnChange);

            if (data.TryGetValue("onChange", out string onChange))
            {
                if (!parserParams.actions.TryGetValue(onChange, out BSMLAction onChangeAction))
                    throw new Exception("on-change action '" + onChange + "' not found");

                listSetting.onChange = onChangeAction;
            }

            if (data.TryGetValue("value", out string value))
            {
                if (!parserParams.values.TryGetValue(value, out BSMLValue associatedValue))
                    throw new Exception("value '" + value + "' not found");

                listSetting.associatedValue = associatedValue;
            }

            if (data.TryGetValue("options", out string options))
            {
                if (!parserParams.values.TryGetValue(options, out BSMLValue values))
                    throw new Exception("options '" + options + "' not found");

                listSetting.values = values.GetValue() as List<object>;
            }
            else
            {
                throw new Exception("list must have associated options");
            }

            parserParams.AddEvent(data.TryGetValue("setEvent", out string setEvent) ? setEvent : "apply", listSetting.ApplyValue);
            parserParams.AddEvent(data.TryGetValue("getEvent", out string getEvent) ? getEvent : "cancel", listSetting.ReceiveValue);

            listSetting.Setup();
        }
    }
}