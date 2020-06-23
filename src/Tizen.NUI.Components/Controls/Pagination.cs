﻿/*
 * Copyright(c) 2019 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Tizen.NUI.BaseComponents;

namespace Tizen.NUI.Components
{
    /// <summary>
    /// Pagination shows the number of pages available and the currently active page.
    /// </summary>
    /// <since_tizen> 6 </since_tizen>
    /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Pagination: Control
    {
        private VisualView container;

        private List<ImageVisual> indicatorList = new List<ImageVisual>();

        private int indicatorCount = 0;
        private int selectedIndex = -1;

        static Pagination() { }

        /// <summary>
        /// Creates a new instance of a Pagination.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
	    /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
	    [EditorBrowsable(EditorBrowsableState.Never)]
        public Pagination() : base()
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance of a Pagination using style.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Pagination(string style) : base(style)
        {
            Initialize();
        }

        /// <summary>
        /// Creates a new instance of a Pagination using style.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Pagination(PaginationStyle paginationStyle) : base(paginationStyle)
        {
            Initialize();
        }

        /// <summary>
        /// Get style of pagination.
        /// </summary>
        /// This will be public opened in tizen_next after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new PaginationStyle Style => ViewStyle as PaginationStyle;

        /// <summary>
        /// Gets or sets the size of the indicator.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Size IndicatorSize
        {
            get
            {
                return Style?.IndicatorSize;
            }
            set
            {
                if (value == null || Style == null)
                {
                    return;
                }
                Style.IndicatorSize = value;
                UpdateVisual();
            }
        }

        /// <summary>
        /// Gets or sets the background resource of indicator.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Selector<string> IndicatorImageURL
        {
            get
            {
                return Style?.IndicatorImageURL;
            }
            set
            {
                if (value == null || Style == null)
                {
                    return;
                }
                Style.IndicatorImageURL = value;
                UpdateVisual();
            }
        }

        /// <summary>
        /// Gets or sets the space of the indicator.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int IndicatorSpacing
        {
            get
            {
                return (int)Style?.IndicatorSpacing;
            }
            set
            {
                Style.IndicatorSpacing = value;
                UpdateVisual();
            }
        }


        /// <summary>
        /// Gets or sets the count of the pages/indicators.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int IndicatorCount
        {
            get
            {
                return indicatorCount;
            }
            set
            {
                if (indicatorCount == value || indicatorCount < 0)
                {
                    return;
                }
                if (indicatorCount < value)
                {
                    for (int i = indicatorCount; i < value; i++)
                    {
                        CreateIndicator();
                    }
                }
                else
                {
                    for (int i = value; i < indicatorCount; i++)
                    {
                        ImageVisual indicator = indicatorList[i];
                        container.RemoveVisual("Indicator" + i);
                    }
                    indicatorList.RemoveRange(value, indicatorCount - value);
                    //if(value <= 0)
                    //{
                    //    container.RemoveVisual("SelectIndicator");
                    //}
                    //else
                    if (selectedIndex >= value)
                    {
                        selectedIndex = 0;
                        SelectIn(indicatorList[selectedIndex]);
                    }
                }
                indicatorCount = value;

                UpdateContainer();
            }
        }

        /// <summary>
        /// Gets or sets the index of the select indicator.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (selectedIndex == value)
                {
                    return;
                }
                if (selectedIndex >= 0 && selectedIndex < indicatorCount)
                {
                    SelectOut(indicatorList[selectedIndex]);
                }
                selectedIndex = value;
                if (selectedIndex >= 0 && selectedIndex < indicatorCount)
                {
                    SelectIn(indicatorList[selectedIndex]);
                }
            }
        }

        /// <summary>
        /// Retrieves the position of a indicator by index.
        /// </summary>
        /// <param name="index">Indicator index</param>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Position GetIndicatorPosition(int index)
        {
            if (index < 0 || index >= indicatorList.Count)
            {
                return null;
            }
            return new Position(indicatorList[index].Position.X + container.PositionX, indicatorList[index].Position.Y + container.PositionY);
        }

        /// <summary>
        /// You can override it to do your select out operation.
        /// </summary>
        /// <param name="selectOutIndicator">The indicator will be selected out</param>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void SelectOut(VisualMap selectOutIndicator)
        {
            if (!(selectOutIndicator is ImageVisual visual)) return;
            visual.URL = Style?.IndicatorImageURL.Normal;
            visual.Opacity = 0.5f;
        }

        /// <summary>
        /// You can override it to do your select in operation.
        /// </summary>
        /// <param name="selectInIndicator">The indicator will be selected in</param>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual void SelectIn(VisualMap selectInIndicator)
        {
            if (!(selectInIndicator is ImageVisual visual)) return;
            visual.URL = Style?.IndicatorImageURL.Selected;
            visual.Opacity = 1.0f;
        }

        /// <summary>
        /// you can override it to create your own default style.
        /// </summary>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override ViewStyle CreateViewStyle()
        {
            return new PaginationStyle();
        }

        /// <summary>
        /// you can override it to clean-up your own resources.
        /// </summary>
        /// <param name="type">DisposeTypes</param>
        /// <since_tizen> 6 </since_tizen>
        /// This will be public opened in tizen_5.5 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void Dispose(DisposeTypes type)
        {
            if (disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                container.RemoveAll();
                indicatorList.Clear();

                this.Remove(container);
                container.Dispose();
                container = null;
            }

            base.Dispose(type);
        }

        private void Initialize()
        {
            container = new VisualView()
            {
                Name = "Container",
                ParentOrigin = Tizen.NUI.ParentOrigin.CenterLeft,
                PivotPoint = Tizen.NUI.PivotPoint.CenterLeft,
                PositionUsesPivotPoint = true,
            };
            this.Add(container);
        }

        private void CreateIndicator()
        {
            if (Style == null)
            {
                return;
            }
            ImageVisual indicator = new ImageVisual
            {
                URL = Style.IndicatorImageURL.Normal,
                Size = new Size2D((int)Style.IndicatorSize.Width, (int)Style.IndicatorSize.Height),
                Opacity = 0.5f,
            };
            indicator.Position = new Position2D((int)(Style.IndicatorSize.Width + Style.IndicatorSpacing) * indicatorList.Count, 0);
            container.AddVisual("Indicator" + indicatorList.Count, indicator);
            indicatorList.Add(indicator);
        }

        private void UpdateContainer()
        {
            if (Style == null)
            {
                return;
            }
            if (indicatorList.Count > 0)
            {
                container.SizeWidth = (Style.IndicatorSize.Width + Style.IndicatorSpacing) * indicatorList.Count - Style.IndicatorSpacing;
            }
            else
            {
                container.SizeWidth = 0;
            }
            container.SizeHeight = Style.IndicatorSize.Height;
            container.PositionX = (int)((this.SizeWidth - container.SizeWidth) / 2);
        }

        private void UpdateVisual()
        {
            if (null == Style.IndicatorSize) return;
            if (null == Style.IndicatorImageURL) return;
            if (indicatorCount < 0) return;

            for (int i = 0; i < indicatorList.Count; i++)
            {
                ImageVisual indicator = indicatorList[i];
                indicator.URL = Style.IndicatorImageURL.Normal;
                indicator.Size = new Size2D((int)Style.IndicatorSize.Width, (int)Style.IndicatorSize.Height);
                indicator.Position = new Position2D((int)(Style.IndicatorSize.Width + Style.IndicatorSpacing) * i, 0);
            }
        }
    }
}
