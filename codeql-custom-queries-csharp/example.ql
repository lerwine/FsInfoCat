/**
 * This is an automatically generated file
 * @name Hello world
 * @kind problem
 * @problem.severity warning
 * @id csharp/example/hello-world
 */

import csharp

from Interface i
where i.getNamespace().getFullName() = "FsInfoCat.Model"
select i.toStringWithTypes(), i.getABaseInterface().toStringWithTypes()
