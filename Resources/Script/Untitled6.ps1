$QueryResults = @'
1	IFileSystem	IValidatableObject
2	IFileSystem	IHasSimpleIdentifier
3	IFileSystem	IDbEntity
4	IFileSystem	IFileSystemRow
5	IFileSystem	IFileSystemProperties
7	ISubdirectory	IValidatableObject
8	ISubdirectory	IHasSimpleIdentifier
9	ISubdirectory	IDbEntity
10	ISubdirectory	IDbFsItemRow
11	ISubdirectory	IDbFsItem
12	ISubdirectory	ISubdirectoryRow
14	IComparison	IValidatableObject
15	IComparison	IHasCompoundIdentifier
16	IComparison	IHasIdentifierPair
17	IComparison	ISynchronizable
18	IComparison	IHasMembershipKeyReference
19	IComparison	IDbEntity
21	IComparison	IHasMembershipKeyReference<IFile, IFile>
22	IFile	IValidatableObject
23	IFile	IHasSimpleIdentifier
24	IFile	IDbEntity
25	IFile	IDbFsItemRow
26	IFile	IDbFsItem
27	IFile	IFileRow
30	IAudioPropertiesListItem	IValidatableObject
31	IAudioPropertiesListItem	IAudioProperties
32	IAudioPropertiesListItem	IHasSimpleIdentifier
33	IAudioPropertiesListItem	IDbEntity
34	IAudioPropertiesListItem	IPropertiesRow
35	IAudioPropertiesListItem	IPropertiesListItem
36	IAudioPropertiesListItem	IAudioPropertiesRow
40	IAudioPropertySet	IValidatableObject
41	IAudioPropertySet	IAudioProperties
42	IAudioPropertySet	IHasSimpleIdentifier
43	IAudioPropertySet	IDbEntity
44	IAudioPropertySet	IPropertiesRow
45	IAudioPropertySet	IPropertySet
46	IAudioPropertySet	IAudioPropertiesRow
50	IBinaryPropertySet	IValidatableObject
51	IBinaryPropertySet	IHasSimpleIdentifier
52	IBinaryPropertySet	IDbEntity
54	ICrawlConfigReportItem	IValidatableObject
55	ICrawlConfigReportItem	ICrawlConfigurationListItem
56	ICrawlConfigReportItem	IHasSimpleIdentifier
57	ICrawlConfigReportItem	IDbEntity
58	ICrawlConfigReportItem	ICrawlConfigurationRow
59	ICrawlConfigReportItem	ICrawlSettings
62	ICrawlConfiguration	IValidatableObject
63	ICrawlConfiguration	IHasSimpleIdentifier
64	ICrawlConfiguration	IDbEntity
65	ICrawlConfiguration	ICrawlConfigurationRow
66	ICrawlConfiguration	ICrawlSettings
68	ICrawlConfigurationListItem	IValidatableObject
69	ICrawlConfigurationListItem	IHasSimpleIdentifier
70	ICrawlConfigurationListItem	IDbEntity
71	ICrawlConfigurationListItem	ICrawlConfigurationRow
72	ICrawlConfigurationListItem	ICrawlSettings
74	ICrawlJobListItem	IValidatableObject
75	ICrawlJobListItem	IHasSimpleIdentifier
76	ICrawlJobListItem	IDbEntity
77	ICrawlJobListItem	ICrawlJobLogRow
78	ICrawlJobListItem	ICrawlSettings
79	ICrawlJobListItem	ICrawlJob
82	ICrawlJobLog	IValidatableObject
83	ICrawlJobLog	IHasSimpleIdentifier
84	ICrawlJobLog	IDbEntity
85	ICrawlJobLog	ICrawlJobLogRow
86	ICrawlJobLog	ICrawlSettings
87	ICrawlJobLog	ICrawlJob
90	IDocumentPropertiesListItem	IValidatableObject
91	IDocumentPropertiesListItem	IDocumentProperties
92	IDocumentPropertiesListItem	IHasSimpleIdentifier
93	IDocumentPropertiesListItem	IDbEntity
94	IDocumentPropertiesListItem	IPropertiesRow
95	IDocumentPropertiesListItem	IPropertiesListItem
96	IDocumentPropertiesListItem	IDocumentPropertiesRow
100	IDocumentPropertySet	IValidatableObject
101	IDocumentPropertySet	IDocumentProperties
102	IDocumentPropertySet	IHasSimpleIdentifier
103	IDocumentPropertySet	IDbEntity
104	IDocumentPropertySet	IPropertiesRow
105	IDocumentPropertySet	IPropertySet
106	IDocumentPropertySet	IDocumentPropertiesRow
110	IDRMPropertiesListItem	IValidatableObject
111	IDRMPropertiesListItem	IDRMProperties
112	IDRMPropertiesListItem	IHasSimpleIdentifier
113	IDRMPropertiesListItem	IDbEntity
114	IDRMPropertiesListItem	IPropertiesRow
115	IDRMPropertiesListItem	IPropertiesListItem
116	IDRMPropertiesListItem	IDRMPropertiesRow
120	IDRMPropertySet	IValidatableObject
121	IDRMPropertySet	IDRMProperties
122	IDRMPropertySet	IHasSimpleIdentifier
123	IDRMPropertySet	IDbEntity
124	IDRMPropertySet	IPropertiesRow
125	IDRMPropertySet	IPropertySet
126	IDRMPropertySet	IDRMPropertiesRow
130	IFileAccessError	IValidatableObject
131	IFileAccessError	IHasSimpleIdentifier
132	IFileAccessError	IDbEntity
133	IFileAccessError	IAccessError
135	IFileListItemWithAncestorNames	IValidatableObject
136	IFileListItemWithAncestorNames	IHasSimpleIdentifier
137	IFileListItemWithAncestorNames	IDbEntity
138	IFileListItemWithAncestorNames	IDbFsItemRow
139	IFileListItemWithAncestorNames	IDbFsItemAncestorName
140	IFileListItemWithAncestorNames	IDbFsItemListItem
141	IFileListItemWithAncestorNames	IDbFsItemListItemWithAncestorNames
142	IFileListItemWithAncestorNames	IFileRow
143	IFileListItemWithAncestorNames	IFileAncestorName
145	IFileListItemWithBinaryProperties	IValidatableObject
146	IFileListItemWithBinaryProperties	IHasSimpleIdentifier
147	IFileListItemWithBinaryProperties	IDbEntity
148	IFileListItemWithBinaryProperties	IDbFsItemRow
149	IFileListItemWithBinaryProperties	IDbFsItemListItem
150	IFileListItemWithBinaryProperties	IFileRow
152	IFileListItemWithBinaryPropertiesAndAncestorNames	IValidatableObject
153	IFileListItemWithBinaryPropertiesAndAncestorNames	IFileListItemWithAncestorNames
154	IFileListItemWithBinaryPropertiesAndAncestorNames	IHasSimpleIdentifier
155	IFileListItemWithBinaryPropertiesAndAncestorNames	IDbEntity
156	IFileListItemWithBinaryPropertiesAndAncestorNames	IDbFsItemRow
157	IFileListItemWithBinaryPropertiesAndAncestorNames	IDbFsItemAncestorName
158	IFileListItemWithBinaryPropertiesAndAncestorNames	IDbFsItemListItem
159	IFileListItemWithBinaryPropertiesAndAncestorNames	IDbFsItemListItemWithAncestorNames
160	IFileListItemWithBinaryPropertiesAndAncestorNames	IFileRow
161	IFileListItemWithBinaryPropertiesAndAncestorNames	IFileAncestorName
164	IFileSystemListItem	IValidatableObject
165	IFileSystemListItem	IHasSimpleIdentifier
166	IFileSystemListItem	IDbEntity
167	IFileSystemListItem	IFileSystemRow
168	IFileSystemListItem	IFileSystemProperties
170	IGPSPropertiesListItem	IValidatableObject
171	IGPSPropertiesListItem	IGPSProperties
172	IGPSPropertiesListItem	IHasSimpleIdentifier
173	IGPSPropertiesListItem	IDbEntity
174	IGPSPropertiesListItem	IPropertiesRow
175	IGPSPropertiesListItem	IPropertiesListItem
176	IGPSPropertiesListItem	IGPSPropertiesRow
180	IGPSPropertySet	IValidatableObject
181	IGPSPropertySet	IGPSProperties
182	IGPSPropertySet	IHasSimpleIdentifier
183	IGPSPropertySet	IDbEntity
184	IGPSPropertySet	IPropertiesRow
185	IGPSPropertySet	IPropertySet
186	IGPSPropertySet	IGPSPropertiesRow
190	IImagePropertiesListItem	IValidatableObject
191	IImagePropertiesListItem	IImageProperties
192	IImagePropertiesListItem	IHasSimpleIdentifier
193	IImagePropertiesListItem	IDbEntity
194	IImagePropertiesListItem	IPropertiesRow
195	IImagePropertiesListItem	IPropertiesListItem
196	IImagePropertiesListItem	IImagePropertiesRow
200	IImagePropertySet	IValidatableObject
201	IImagePropertySet	IImageProperties
202	IImagePropertySet	IHasSimpleIdentifier
203	IImagePropertySet	IDbEntity
204	IImagePropertySet	IPropertiesRow
205	IImagePropertySet	IPropertySet
206	IImagePropertySet	IImagePropertiesRow
210	IMediaPropertiesListItem	IValidatableObject
211	IMediaPropertiesListItem	IMediaProperties
212	IMediaPropertiesListItem	IHasSimpleIdentifier
213	IMediaPropertiesListItem	IDbEntity
214	IMediaPropertiesListItem	IPropertiesRow
215	IMediaPropertiesListItem	IPropertiesListItem
216	IMediaPropertiesListItem	IMediaPropertiesRow
220	IMediaPropertySet	IValidatableObject
221	IMediaPropertySet	IMediaProperties
222	IMediaPropertySet	IHasSimpleIdentifier
223	IMediaPropertySet	IDbEntity
224	IMediaPropertySet	IPropertiesRow
225	IMediaPropertySet	IPropertySet
226	IMediaPropertySet	IMediaPropertiesRow
230	IMusicPropertiesListItem	IValidatableObject
231	IMusicPropertiesListItem	IMusicProperties
232	IMusicPropertiesListItem	IHasSimpleIdentifier
233	IMusicPropertiesListItem	IDbEntity
234	IMusicPropertiesListItem	IPropertiesRow
235	IMusicPropertiesListItem	IPropertiesListItem
236	IMusicPropertiesListItem	IMusicPropertiesRow
240	IMusicPropertySet	IValidatableObject
241	IMusicPropertySet	IMusicProperties
242	IMusicPropertySet	IHasSimpleIdentifier
243	IMusicPropertySet	IDbEntity
244	IMusicPropertySet	IPropertiesRow
245	IMusicPropertySet	IPropertySet
246	IMusicPropertySet	IMusicPropertiesRow
250	IPersonalFileTag	IValidatableObject
251	IPersonalFileTag	IHasCompoundIdentifier
252	IPersonalFileTag	IHasIdentifierPair
253	IPersonalFileTag	ISynchronizable
254	IPersonalFileTag	IHasMembershipKeyReference
255	IPersonalFileTag	IDbEntity
256	IPersonalFileTag	IItemTagRow
257	IPersonalFileTag	IPersonalTag
258	IPersonalFileTag	IItemTag
259	IPersonalFileTag	IFileTag
261	IPersonalFileTag	IHasMembershipKeyReference<IFile, ITagDefinition>
262	IPersonalFileTag	IHasMembershipKeyReference<IFile, IPersonalTagDefinition>
263	IPersonalSubdirectoryTag	IValidatableObject
264	IPersonalSubdirectoryTag	IHasCompoundIdentifier
265	IPersonalSubdirectoryTag	IHasIdentifierPair
266	IPersonalSubdirectoryTag	ISynchronizable
267	IPersonalSubdirectoryTag	IHasMembershipKeyReference
268	IPersonalSubdirectoryTag	IDbEntity
269	IPersonalSubdirectoryTag	IItemTagRow
270	IPersonalSubdirectoryTag	IPersonalTag
271	IPersonalSubdirectoryTag	IItemTag
272	IPersonalSubdirectoryTag	ISubdirectoryTag
274	IPersonalSubdirectoryTag	IHasMembershipKeyReference<ISubdirectory, IPersonalTagDefinition>
275	IPersonalSubdirectoryTag	IHasMembershipKeyReference<ISubdirectory, ITagDefinition>
276	IPersonalTagDefinition	IValidatableObject
277	IPersonalTagDefinition	IHasSimpleIdentifier
278	IPersonalTagDefinition	IDbEntity
279	IPersonalTagDefinition	ITagDefinitionRow
280	IPersonalTagDefinition	ITagDefinition
283	IPersonalVolumeTag	IValidatableObject
284	IPersonalVolumeTag	IHasCompoundIdentifier
285	IPersonalVolumeTag	IHasIdentifierPair
286	IPersonalVolumeTag	ISynchronizable
287	IPersonalVolumeTag	IHasMembershipKeyReference
288	IPersonalVolumeTag	IDbEntity
289	IPersonalVolumeTag	IItemTagRow
290	IPersonalVolumeTag	IPersonalTag
291	IPersonalVolumeTag	IItemTag
292	IPersonalVolumeTag	IVolumeTag
294	IPersonalVolumeTag	IHasMembershipKeyReference<IVolume, IPersonalTagDefinition>
295	IPersonalVolumeTag	IHasMembershipKeyReference<IVolume, ITagDefinition>
296	IPhotoPropertiesListItem	IValidatableObject
297	IPhotoPropertiesListItem	IPhotoProperties
298	IPhotoPropertiesListItem	IHasSimpleIdentifier
299	IPhotoPropertiesListItem	IDbEntity
300	IPhotoPropertiesListItem	IPropertiesRow
301	IPhotoPropertiesListItem	IPropertiesListItem
302	IPhotoPropertiesListItem	IPhotoPropertiesRow
306	IPhotoPropertySet	IValidatableObject
307	IPhotoPropertySet	IPhotoProperties
308	IPhotoPropertySet	IHasSimpleIdentifier
309	IPhotoPropertySet	IDbEntity
310	IPhotoPropertySet	IPropertiesRow
311	IPhotoPropertySet	IPropertySet
314	IRecordedTVPropertiesListItem	IValidatableObject
315	IRecordedTVPropertiesListItem	IRecordedTVProperties
316	IRecordedTVPropertiesListItem	IHasSimpleIdentifier
317	IRecordedTVPropertiesListItem	IDbEntity
318	IRecordedTVPropertiesListItem	IPropertiesRow
319	IRecordedTVPropertiesListItem	IPropertiesListItem
320	IRecordedTVPropertiesListItem	IRecordedTVPropertiesRow
324	IRecordedTVPropertySet	IValidatableObject
325	IRecordedTVPropertySet	IRecordedTVProperties
326	IRecordedTVPropertySet	IHasSimpleIdentifier
327	IRecordedTVPropertySet	IDbEntity
328	IRecordedTVPropertySet	IPropertiesRow
329	IRecordedTVPropertySet	IPropertySet
330	IRecordedTVPropertySet	IRecordedTVPropertiesRow
334	IRedundancy	IValidatableObject
335	IRedundancy	IHasCompoundIdentifier
336	IRedundancy	IHasIdentifierPair
337	IRedundancy	ISynchronizable
338	IRedundancy	IHasMembershipKeyReference
339	IRedundancy	IDbEntity
341	IRedundancy	IHasMembershipKeyReference<IRedundantSet, IFile>
342	IRedundantSet	IValidatableObject
343	IRedundantSet	IHasSimpleIdentifier
344	IRedundantSet	IDbEntity
345	IRedundantSet	IRedundantSetRow
347	IRedundantSetListItem	IValidatableObject
348	IRedundantSetListItem	IHasSimpleIdentifier
349	IRedundantSetListItem	IDbEntity
350	IRedundantSetListItem	IRedundantSetRow
352	ISharedFileTag	IValidatableObject
353	ISharedFileTag	IHasCompoundIdentifier
354	ISharedFileTag	IHasIdentifierPair
355	ISharedFileTag	ISynchronizable
356	ISharedFileTag	IHasMembershipKeyReference
357	ISharedFileTag	IDbEntity
358	ISharedFileTag	IItemTagRow
359	ISharedFileTag	ISharedTag
360	ISharedFileTag	IItemTag
361	ISharedFileTag	IFileTag
363	ISharedFileTag	IHasMembershipKeyReference<IFile, ITagDefinition>
364	ISharedFileTag	IHasMembershipKeyReference<IFile, ISharedTagDefinition>
365	ISharedSubdirectoryTag	IValidatableObject
366	ISharedSubdirectoryTag	IHasCompoundIdentifier
367	ISharedSubdirectoryTag	IHasIdentifierPair
368	ISharedSubdirectoryTag	ISynchronizable
369	ISharedSubdirectoryTag	IHasMembershipKeyReference
370	ISharedSubdirectoryTag	IDbEntity
371	ISharedSubdirectoryTag	IItemTagRow
372	ISharedSubdirectoryTag	ISharedTag
373	ISharedSubdirectoryTag	IItemTag
374	ISharedSubdirectoryTag	ISubdirectoryTag
376	ISharedSubdirectoryTag	IHasMembershipKeyReference<ISubdirectory, ISharedTagDefinition>
377	ISharedSubdirectoryTag	IHasMembershipKeyReference<ISubdirectory, ITagDefinition>
378	ISharedTagDefinition	IValidatableObject
379	ISharedTagDefinition	IHasSimpleIdentifier
380	ISharedTagDefinition	IDbEntity
381	ISharedTagDefinition	ITagDefinitionRow
382	ISharedTagDefinition	ITagDefinition
385	ISharedVolumeTag	IValidatableObject
386	ISharedVolumeTag	IHasCompoundIdentifier
387	ISharedVolumeTag	IHasIdentifierPair
388	ISharedVolumeTag	ISynchronizable
389	ISharedVolumeTag	IHasMembershipKeyReference
390	ISharedVolumeTag	IDbEntity
391	ISharedVolumeTag	IItemTagRow
392	ISharedVolumeTag	ISharedTag
393	ISharedVolumeTag	IItemTag
394	ISharedVolumeTag	IVolumeTag
396	ISharedVolumeTag	IHasMembershipKeyReference<IVolume, ISharedTagDefinition>
397	ISharedVolumeTag	IHasMembershipKeyReference<IVolume, ITagDefinition>
398	ISubdirectoryAccessError	IValidatableObject
399	ISubdirectoryAccessError	IHasSimpleIdentifier
400	ISubdirectoryAccessError	IDbEntity
401	ISubdirectoryAccessError	IAccessError
403	ISubdirectoryAncestorName	IHasSimpleIdentifier
404	ISubdirectoryAncestorName	IDbFsItemAncestorName
406	ISubdirectoryListItem	IValidatableObject
407	ISubdirectoryListItem	IHasSimpleIdentifier
408	ISubdirectoryListItem	IDbEntity
409	ISubdirectoryListItem	IDbFsItemRow
410	ISubdirectoryListItem	IDbFsItemListItem
411	ISubdirectoryListItem	ISubdirectoryRow
413	ISubdirectoryListItemWithAncestorNames	IValidatableObject
414	ISubdirectoryListItemWithAncestorNames	ISubdirectoryAncestorName
415	ISubdirectoryListItemWithAncestorNames	ISubdirectoryListItem
416	ISubdirectoryListItemWithAncestorNames	IHasSimpleIdentifier
417	ISubdirectoryListItemWithAncestorNames	IDbEntity
418	ISubdirectoryListItemWithAncestorNames	IDbFsItemRow
419	ISubdirectoryListItemWithAncestorNames	IDbFsItemAncestorName
420	ISubdirectoryListItemWithAncestorNames	IDbFsItemListItem
421	ISubdirectoryListItemWithAncestorNames	IDbFsItemListItemWithAncestorNames
422	ISubdirectoryListItemWithAncestorNames	ISubdirectoryRow
426	ISummaryPropertiesListItem	IValidatableObject
427	ISummaryPropertiesListItem	ISummaryProperties
428	ISummaryPropertiesListItem	IHasSimpleIdentifier
429	ISummaryPropertiesListItem	IDbEntity
430	ISummaryPropertiesListItem	IPropertiesRow
431	ISummaryPropertiesListItem	IPropertiesListItem
432	ISummaryPropertiesListItem	ISummaryPropertiesRow
436	ISummaryPropertySet	IValidatableObject
437	ISummaryPropertySet	ISummaryProperties
438	ISummaryPropertySet	IHasSimpleIdentifier
439	ISummaryPropertySet	IDbEntity
440	ISummaryPropertySet	IPropertiesRow
441	ISummaryPropertySet	IPropertySet
442	ISummaryPropertySet	ISummaryPropertiesRow
446	ISymbolicName	IValidatableObject
447	ISymbolicName	IHasSimpleIdentifier
448	ISymbolicName	IDbEntity
449	ISymbolicName	ISymbolicNameRow
451	ISymbolicNameListItem	IValidatableObject
452	ISymbolicNameListItem	IHasSimpleIdentifier
453	ISymbolicNameListItem	IDbEntity
454	ISymbolicNameListItem	ISymbolicNameRow
456	IVideoPropertiesListItem	IValidatableObject
457	IVideoPropertiesListItem	IVideoProperties
458	IVideoPropertiesListItem	IHasSimpleIdentifier
459	IVideoPropertiesListItem	IDbEntity
460	IVideoPropertiesListItem	IPropertiesRow
461	IVideoPropertiesListItem	IPropertiesListItem
462	IVideoPropertiesListItem	IVideoPropertiesRow
466	IVideoPropertySet	IValidatableObject
467	IVideoPropertySet	IVideoProperties
468	IVideoPropertySet	IHasSimpleIdentifier
469	IVideoPropertySet	IDbEntity
470	IVideoPropertySet	IPropertiesRow
471	IVideoPropertySet	IPropertySet
472	IVideoPropertySet	IVideoPropertiesRow
476	IVolume	IValidatableObject
477	IVolume	IHasSimpleIdentifier
478	IVolume	IDbEntity
479	IVolume	IVolumeRow
481	IVolumeAccessError	IValidatableObject
482	IVolumeAccessError	IHasSimpleIdentifier
483	IVolumeAccessError	IDbEntity
484	IVolumeAccessError	IAccessError
486	IVolumeListItem	IValidatableObject
487	IVolumeListItem	IHasSimpleIdentifier
488	IVolumeListItem	IDbEntity
489	IVolumeListItem	IVolumeRow
491	IVolumeListItemWithFileSystem	IValidatableObject
492	IVolumeListItemWithFileSystem	IVolumeListItem
493	IVolumeListItemWithFileSystem	IHasSimpleIdentifier
494	IVolumeListItemWithFileSystem	IDbEntity
495	IVolumeListItemWithFileSystem	IVolumeRow
498	IItemTagListItem	IValidatableObject
499	IItemTagListItem	IHasCompoundIdentifier
500	IItemTagListItem	IHasIdentifierPair
501	IItemTagListItem	IDbEntity
502	IItemTagListItem	IItemTagRow
504	ITagDefinitionListItem	IValidatableObject
505	ITagDefinitionListItem	IHasSimpleIdentifier
506	ITagDefinitionListItem	IDbEntity
507	ITagDefinitionListItem	ITagDefinitionRow
520	IHasIdentifierPair	IHasCompoundIdentifier
523	IHasMembershipKeyReference	IHasCompoundIdentifier
524	IHasMembershipKeyReference	IHasIdentifierPair
525	IHasMembershipKeyReference	ISynchronizable
526	IDbEntity	IValidatableObject
527	IAccessError	IValidatableObject
528	IAccessError	IHasSimpleIdentifier
529	IAccessError	IDbEntity
530	ICrawlConfigurationRow	IValidatableObject
531	ICrawlConfigurationRow	IHasSimpleIdentifier
532	ICrawlConfigurationRow	IDbEntity
533	ICrawlConfigurationRow	ICrawlSettings
534	ICrawlJobLogRow	IValidatableObject
535	ICrawlJobLogRow	IHasSimpleIdentifier
536	ICrawlJobLogRow	IDbEntity
537	ICrawlJobLogRow	ICrawlSettings
538	ICrawlJobLogRow	ICrawlJob
540	IDbFsItemRow	IValidatableObject
541	IDbFsItemRow	IHasSimpleIdentifier
542	IDbFsItemRow	IDbEntity
543	IFileSystemRow	IValidatableObject
544	IFileSystemRow	IHasSimpleIdentifier
545	IFileSystemRow	IDbEntity
546	IFileSystemRow	IFileSystemProperties
547	IItemTagRow	IValidatableObject
548	IItemTagRow	IHasCompoundIdentifier
549	IItemTagRow	IHasIdentifierPair
550	IItemTagRow	IDbEntity
551	IPropertiesRow	IValidatableObject
552	IPropertiesRow	IHasSimpleIdentifier
553	IPropertiesRow	IDbEntity
554	IRedundantSetRow	IValidatableObject
555	IRedundantSetRow	IHasSimpleIdentifier
556	IRedundantSetRow	IDbEntity
557	ISymbolicNameRow	IValidatableObject
558	ISymbolicNameRow	IHasSimpleIdentifier
559	ISymbolicNameRow	IDbEntity
560	ITagDefinitionRow	IValidatableObject
561	ITagDefinitionRow	IHasSimpleIdentifier
562	ITagDefinitionRow	IDbEntity
563	IVolumeRow	IValidatableObject
564	IVolumeRow	IHasSimpleIdentifier
565	IVolumeRow	IDbEntity
566	IDbFsItemAncestorName	IHasSimpleIdentifier
567	IDbFsItemListItem	IValidatableObject
568	IDbFsItemListItem	IHasSimpleIdentifier
569	IDbFsItemListItem	IDbEntity
570	IDbFsItemListItem	IDbFsItemRow
571	IDbFsItemListItemWithAncestorNames	IValidatableObject
572	IDbFsItemListItemWithAncestorNames	IHasSimpleIdentifier
573	IDbFsItemListItemWithAncestorNames	IDbEntity
574	IDbFsItemListItemWithAncestorNames	IDbFsItemRow
575	IDbFsItemListItemWithAncestorNames	IDbFsItemAncestorName
576	IDbFsItemListItemWithAncestorNames	IDbFsItemListItem
577	IDbFsItem	IValidatableObject
578	IDbFsItem	IHasSimpleIdentifier
579	IDbFsItem	IDbEntity
580	IDbFsItem	IDbFsItemRow
581	ISharedTag	IValidatableObject
582	ISharedTag	IHasCompoundIdentifier
583	ISharedTag	IHasIdentifierPair
584	ISharedTag	IDbEntity
585	ISharedTag	IItemTagRow
586	ISharedTag	IItemTag
587	IPersonalTag	IValidatableObject
588	IPersonalTag	IHasCompoundIdentifier
589	IPersonalTag	IHasIdentifierPair
590	IPersonalTag	IDbEntity
591	IPersonalTag	IItemTagRow
592	IPersonalTag	IItemTag
593	ISubdirectoryRow	IValidatableObject
594	ISubdirectoryRow	IHasSimpleIdentifier
595	ISubdirectoryRow	IDbEntity
596	ISubdirectoryRow	IDbFsItemRow
597	IFileRow	IValidatableObject
598	IFileRow	IHasSimpleIdentifier
599	IFileRow	IDbEntity
600	IFileRow	IDbFsItemRow
601	IFileAncestorName	IHasSimpleIdentifier
602	IFileAncestorName	IDbFsItemAncestorName
603	ITagDefinition	IValidatableObject
604	ITagDefinition	IHasSimpleIdentifier
605	ITagDefinition	IDbEntity
606	ITagDefinition	ITagDefinitionRow
608	IItemTag	IValidatableObject
609	IItemTag	IHasCompoundIdentifier
610	IItemTag	IHasIdentifierPair
611	IItemTag	IDbEntity
612	IItemTag	IItemTagRow
613	IFileTag	IValidatableObject
614	IFileTag	IHasCompoundIdentifier
615	IFileTag	IHasIdentifierPair
616	IFileTag	ISynchronizable
617	IFileTag	IHasMembershipKeyReference
618	IFileTag	IDbEntity
619	IFileTag	IItemTagRow
620	IFileTag	IItemTag
621	IFileTag	IHasMembershipKeyReference<IFile, ITagDefinition>
622	ISubdirectoryTag	IValidatableObject
623	ISubdirectoryTag	IHasCompoundIdentifier
624	ISubdirectoryTag	IHasIdentifierPair
625	ISubdirectoryTag	ISynchronizable
626	ISubdirectoryTag	IHasMembershipKeyReference
627	ISubdirectoryTag	IDbEntity
628	ISubdirectoryTag	IItemTagRow
629	ISubdirectoryTag	IItemTag
630	ISubdirectoryTag	IHasMembershipKeyReference<ISubdirectory, ITagDefinition>
631	IVolumeTag	IValidatableObject
632	IVolumeTag	IHasCompoundIdentifier
633	IVolumeTag	IHasIdentifierPair
634	IVolumeTag	ISynchronizable
635	IVolumeTag	IHasMembershipKeyReference
636	IVolumeTag	IDbEntity
637	IVolumeTag	IItemTagRow
638	IVolumeTag	IItemTag
639	IVolumeTag	IHasMembershipKeyReference<IVolume, ITagDefinition>
640	IPropertiesListItem	IValidatableObject
641	IPropertiesListItem	IHasSimpleIdentifier
642	IPropertiesListItem	IDbEntity
643	IPropertiesListItem	IPropertiesRow
644	IPropertySet	IValidatableObject
645	IPropertySet	IHasSimpleIdentifier
646	IPropertySet	IDbEntity
647	IPropertySet	IPropertiesRow
648	ISummaryPropertiesRow	IValidatableObject
649	ISummaryPropertiesRow	ISummaryProperties
650	ISummaryPropertiesRow	IHasSimpleIdentifier
651	ISummaryPropertiesRow	IDbEntity
652	ISummaryPropertiesRow	IPropertiesRow
655	IDocumentPropertiesRow	IValidatableObject
656	IDocumentPropertiesRow	IDocumentProperties
657	IDocumentPropertiesRow	IHasSimpleIdentifier
658	IDocumentPropertiesRow	IDbEntity
659	IDocumentPropertiesRow	IPropertiesRow
662	IAudioPropertiesRow	IValidatableObject
663	IAudioPropertiesRow	IAudioProperties
664	IAudioPropertiesRow	IHasSimpleIdentifier
665	IAudioPropertiesRow	IDbEntity
666	IAudioPropertiesRow	IPropertiesRow
669	IDRMPropertiesRow	IValidatableObject
670	IDRMPropertiesRow	IDRMProperties
671	IDRMPropertiesRow	IHasSimpleIdentifier
672	IDRMPropertiesRow	IDbEntity
673	IDRMPropertiesRow	IPropertiesRow
676	IGPSPropertiesRow	IValidatableObject
677	IGPSPropertiesRow	IGPSProperties
678	IGPSPropertiesRow	IHasSimpleIdentifier
679	IGPSPropertiesRow	IDbEntity
680	IGPSPropertiesRow	IPropertiesRow
683	IImagePropertiesRow	IValidatableObject
684	IImagePropertiesRow	IImageProperties
685	IImagePropertiesRow	IHasSimpleIdentifier
686	IImagePropertiesRow	IDbEntity
687	IImagePropertiesRow	IPropertiesRow
690	IMediaPropertiesRow	IValidatableObject
691	IMediaPropertiesRow	IMediaProperties
692	IMediaPropertiesRow	IHasSimpleIdentifier
693	IMediaPropertiesRow	IDbEntity
694	IMediaPropertiesRow	IPropertiesRow
697	IMusicPropertiesRow	IValidatableObject
698	IMusicPropertiesRow	IMusicProperties
699	IMusicPropertiesRow	IHasSimpleIdentifier
700	IMusicPropertiesRow	IDbEntity
701	IMusicPropertiesRow	IPropertiesRow
704	IPhotoPropertiesRow	IValidatableObject
705	IPhotoPropertiesRow	IPhotoProperties
706	IPhotoPropertiesRow	IHasSimpleIdentifier
707	IPhotoPropertiesRow	IDbEntity
708	IPhotoPropertiesRow	IPropertiesRow
711	IRecordedTVPropertiesRow	IValidatableObject
712	IRecordedTVPropertiesRow	IRecordedTVProperties
713	IRecordedTVPropertiesRow	IHasSimpleIdentifier
714	IRecordedTVPropertiesRow	IDbEntity
715	IRecordedTVPropertiesRow	IPropertiesRow
718	IVideoPropertiesRow	IValidatableObject
719	IVideoPropertiesRow	IVideoProperties
720	IVideoPropertiesRow	IHasSimpleIdentifier
721	IVideoPropertiesRow	IDbEntity
722	IVideoPropertiesRow	IPropertiesRow
1009	IHasMembershipKeyReference<TEntity1, TEntity2>	IHasCompoundIdentifier
1010	IHasMembershipKeyReference<TEntity1, TEntity2>	IHasIdentifierPair
1011	IHasMembershipKeyReference<TEntity1, TEntity2>	ISynchronizable
1012	IHasMembershipKeyReference<TEntity1, TEntity2>	IHasMembershipKeyReference
'@
class InterfaceDef {
    [string]$Name;
    [System.Collections.ObjectModel.Collection[InterfaceDef]]$BaseTypes = [System.Collections.ObjectModel.Collection[InterfaceDef]]::new();
    [bool] InheritsFrom([InterfaceDef]$Other) {
        foreach ($b in $this.BaseTypes) {
            if ([object]::ReferenceEquals($b, $Other) -or $b.InheritsFrom($Other)) { return $true }
        }
        return $false;
    }
}
$TypeDict = [System.Collections.Generic.Dictionary[string,InterfaceDef]]::new([StringComparer]::CurrentCulture);
($QueryResults -split '[\r\n]+') | ForEach-Object {
    ($Number, $InterfaceName, $BaseName) = $_.Split("`t", 3);
    if (-not ($InterfaceName.Contains('`') -or $InterfaceName.Contains('<'))) {
        [InterfaceDef]$Interface = $null;
        if (-not $TypeDict.TryGetValue($InterfaceName, [ref]$Interface)) {
            $Interface = [InterfaceDef]@{ Name = $InterfaceName };
            $TypeDict.Add($InterfaceName, $Interface);
        }
        if (-not ($BaseName.Contains('`') -or $BaseName.Contains('<'))) {
            [InterfaceDef]$Base = $null;
            if (-not $TypeDict.TryGetValue($BaseName, [ref]$Base)) {
                $Base = [InterfaceDef]@{ Name = $BaseName };
                $TypeDict.Add($BaseName, $Base);
            }
            $Interface.BaseTypes.Add($Base);
        }
    }
}
Function Write-ClassDiagramLine {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [System.Collections.Generic.Dictionary[string,InterfaceDef]]$Dictionary
    )

    Process {
        [InterfaceDef]$Interface = $null;
        if ($Dictionary.TryGetValue($Name, [ref]$Interface)) {
            if ($Interface.BaseTypes.Count -gt 0) {
                ($Interface.BaseTypes | Sort-Object -Property 'Name') | Write-ClassDiagramLine -Dictionary $Dictionary;
                "    class $Name"
                foreach ($b in $Interface.BaseTypes) {
                    if ($null -eq ($Interface.BaseTypes | Where-Object { $_.InheritsFrom($b) } | Select-Object -First 1)) {
                        "    $Name --|> $($b.Name)"
                    }
                }
            } else {
                "    class $Name"
            }
            $Dictionary.Remove($Name) | Out-Null;
        }
    }
}

Function Write-SeeAlsos {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [System.Collections.Generic.Dictionary[string,InterfaceDef]]$Dictionary,

        [System.Collections.Generic.Dictionary[string,System.Collections.ObjectModel.Collection[string]]]$SeeAlsoDict
    )

    Begin {
        if ($PSBoundParameters.ContainsKey('SeeAlsoDict')) {
            $Local:SeeAlsos = $SeeAlsoDict;
        } else {
            $Local:SeeAlsos = [System.Collections.Generic.Dictionary[string,System.Collections.ObjectModel.Collection[string]]]::new([StringComparer]::CurrentCulture);
        }
    }

    Process {
        [InterfaceDef]$Interface = $null;
        if ($Dictionary.TryGetValue($Name, [ref]$Interface)) {
            if ($Interface.BaseTypes.Count -gt 0) {
                ($Interface.BaseTypes | Sort-Object -Property 'Name') | Write-SeeAlsos -Dictionary $Dictionary -SeeAlsoDict $Local:SeeAlsos;

                foreach ($b in $Interface.BaseTypes) {
                    if ($null -eq ($Interface.BaseTypes | Where-Object { $_.InheritsFrom($b) } | Select-Object -First 1)) {
                        [System.Collections.ObjectModel.Collection[string]]$Arr = $null;
                        if ($Local:SeeAlsos.TryGetValue($b.Name, [ref]$Arr)) {
                            if (-not $Arr.Contains($Name)) { $Arr.Add($Name) }
                        } else {
                            [System.Collections.ObjectModel.Collection[string]]$Arr = @($Name);
                            $Local:SeeAlsos.Add($b.Name, $Arr);
                        }
                    }
                }
            }
            $Dictionary.Remove($Name) | Out-Null;
        }
    }

    End {
        if (-not $PSBoundParameters.ContainsKey('SeeAlsoDict')) {
            $Local:SeeAlsos.Keys | Sort-Object | ForEach-Object {
                '';
                $Local:SeeAlsos[$_] | ForEach-Object {
                    "    /// <seealso cref=`"$_`" />"
                }
                "    public interface $_ { }"
            }
        }
    }
}
# $Dict = [System.Collections.Generic.Dictionary[string,InterfaceDef]]::new([StringComparer]::CurrentCulture);
# $TypeDict.Keys | ForEach-Object { $Dict.Add($_, $TypeDict[$_]) }
#@($TypeDict.Keys | Sort-Object) | Write-ClassDiagramLine -Dictionary $Dict;
$Dict = [System.Collections.Generic.Dictionary[string,InterfaceDef]]::new([StringComparer]::CurrentCulture);
$TypeDict.Keys | ForEach-Object { $Dict.Add($_, $TypeDict[$_]) }
@($TypeDict.Keys | Sort-Object) | Write-SeeAlsos -Dictionary $Dict;

