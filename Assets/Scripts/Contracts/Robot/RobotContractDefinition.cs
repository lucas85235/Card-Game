using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Numerics;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Solidity.Contracts.Robot.ContractDefinition
{
    // function totalSupply() external view returns (uint256)

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage {}

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public BigInteger totalSupply { get; set; }
    }

    // function robotsList(address from) public view returns (uint256[] memory)

    [Function("robotsList", "uint256[]")]
    public class RobotsListFunctionBase : FunctionMessage 
    {
        [Parameter("address", "from", 1)]
        public string From { get; set; }
    }

    [FunctionOutput]
    public class RobotsListOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256[]", 1)]
        public List<BigInteger> robotsInWallet { get; set; }
    }

    // function getRobotParts(uint256 tokenID) public view returns (Attributes memory)

    [Function("getRobotParts", "string")]
    public class GetRobotPartsFunctionBase : FunctionMessage 
    {
        [Parameter("uint256", "tokenID", 1)]
        public int TokenID { get; set; }
    }

    [FunctionOutput]
    public class GetRobotPartsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "head", 1)]
        public int Head { get; set; }

        [Parameter("uint256", "body", 2)]
        public int Body { get; set; }

        [Parameter("uint256", "armsLeft", 3)]
        public int ArmsLeft { get; set; }

        [Parameter("uint256", "armsRight", 4)]
        public int ArmsRight { get; set; }

        [Parameter("uint256", "legs", 5)]
        public int Legs { get; set; }
    }
}
