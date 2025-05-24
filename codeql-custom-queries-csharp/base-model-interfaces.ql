/**
 * This is an automatically generated file
 * @name Base Model Interfaces
 * @kind problem
 * @problem.severity recommendation
 * @id csharp/fsinfocat/base-model-interfaces
 */

import csharp

from Interface interface, Interface baseInterface
where
    interface.getNamespace().getFullName() = "FsInfoCat.Model" and
    baseInterface.getNamespace().getFullName() = "FsInfoCat.Model" and
    baseInterface = interface.getABaseInterface()
select interface.toStringWithTypes(), baseInterface.toStringWithTypes()
