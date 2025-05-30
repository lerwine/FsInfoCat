/**
 * @id csharp/fsinfocat/upstream-model-interfaces
 * @name Upstream Model Interfaces
 * @description Finds interface inheritances for interfaces in FsInfoCat.Upstream.Model
 */

import csharp

from Interface interface, Interface baseInterface
where
    interface.getNamespace().getFullName() = "FsInfoCat.Upstream.Model" and
    (baseInterface.getNamespace().getFullName() = "FsInfoCat.Upstream.Model" or baseInterface.getNamespace().getFullName() = "FsInfoCat.Model") and
    interface.getABaseInterface() = baseInterface
select interface.toStringWithTypes(), baseInterface.getNamespace().getFullName() + "." + baseInterface.toStringWithTypes()
