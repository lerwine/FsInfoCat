/**
 * This is an automatically generated file
 * @name Local Model Interfaces
 * @kind problem
 * @problem.severity warning
 * @id csharp/fsinfocat/local-model-interfaces
 */

import csharp

from Interface interface, Interface baseInterface
where
    interface.getNamespace().getFullName() = "FsInfoCat.Local.Model" and
    (baseInterface.getNamespace().getFullName() = "FsInfoCat.Local.Model" or baseInterface.getNamespace().getFullName() = "FsInfoCat.Model") and
    interface.getABaseInterface() = baseInterface
select interface.toStringWithTypes(), baseInterface.getNamespace().getFullName() + "." + baseInterface.toStringWithTypes()

