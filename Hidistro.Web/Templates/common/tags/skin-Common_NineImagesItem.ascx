<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
    <div class="ninefigureoneinfoimg">
                                                <p class="info mb5"><%# Eval("ShareDesc") %></p>
                                                <ul class="clearfix y-imglist">
                                                    <li class="img">
                                                       <%# Eval("image1").ToString().Length>5?"<img src='"+ Eval("image1").ToString()+"'/>":"<p>无图片</p>"%>
                                                    </li>
                                                    <li class="img">
                                                        <%# Eval("image2").ToString().Length>5?"<img src='"+ Eval("image2").ToString()+"'/>":"<p>无图片</p>"%>
                                                    </li>
                                                    <li class="img">
                                                        <%# Eval("image3").ToString().Length>5?"<img src='"+ Eval("image3").ToString()+"'/>":"<p>无图片</p>"%>
                                                    </li>
                                                    <li class="img">
                                                        <%# Eval("image4").ToString().Length>5?"<img src='"+ Eval("image4").ToString()+"'/>":"<p>无图片</p>"%>
                                                    </li>
                                                    <li class="img">
                                                        <%# Eval("image5").ToString().Length>5?"<img src='"+ Eval("image5").ToString()+"'/>":"<p>无图片</p>"%>
                                                    </li>
                                                    <li class="img">
                                                        <%# Eval("image6").ToString().Length>5?"<img src='"+ Eval("image6").ToString()+"'/>":"<p>无图片</p>"%>
                                                    </li>
                                                    <li class="img">
                                                        <%# Eval("image7").ToString().Length>5?"<img src='"+ Eval("image7").ToString()+"'/>":"<p>无图片</p>"%>
                                                    </li>
                                                    <li class="img">
                                                        <%# Eval("image8").ToString().Length>5?"<img src='"+ Eval("image8").ToString()+"'/>":"<p>无图片</p>"%>
                                                    </li>
                                                     <li class="img">
                                                        <%# Eval("image9").ToString().Length>5?"<img src='"+ Eval("image9").ToString()+"'/>":"<p>无图片</p>"%>
                                                    </li>
                                                </ul>
                                            </div>
