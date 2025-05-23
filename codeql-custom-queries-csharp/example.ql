/**
 * This is an automatically generated file
 * @name Hello world
 * @kind problem
 * @problem.severity warning
 * @id csharp/example/hello-world
 */

import csharp

from Interface i, Interface b
where
    i.getNamespace().getFullName() = "FsInfoCat.Local.Model" and
    b.getNamespace().getFullName() = "FsInfoCat.Model" and
    i.getABaseInterface() = b
select i.toStringWithTypes(), b.toStringWithTypes()
