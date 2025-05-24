/**
 * This is an automatically generated file
 * @name Upstream Model Interfaces
 * @kind problem
 * @problem.severity warning
 * @id csharp/fsinfocat/upstream-model-interfaces
 */

import csharp

from Interface interface, Interface baseInterface
where
    interface.getNamespace().getFullName() = "FsInfoCat.Upstream.Model" and
    (baseInterface.getNamespace().getFullName() = "FsInfoCat.Upstream.Model" or baseInterface.getNamespace().getFullName() = "FsInfoCat.Model") and
    interface.getABaseInterface() = baseInterface
select interface.toStringWithTypes(), baseInterface.getNamespace().getFullName() + "." + baseInterface.toStringWithTypes()
