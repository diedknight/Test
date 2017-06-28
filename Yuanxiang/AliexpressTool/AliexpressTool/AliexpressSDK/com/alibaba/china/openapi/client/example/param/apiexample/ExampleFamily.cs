using com.alibaba.openapi.client.primitive;
using com.alibaba.openapi.client.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace com.alibaba.china.openapi.client.example.param.apiexample
{
[DataContract(Namespace = "com.alibaba.openapi.client")]
class ExampleFamily {

       [DataMember(Order = 1)]
    private int? familyNumber;
    
        /**
       * @return 家庭编号
    */
        public int? getFamilyNumber() {
               	return familyNumber;
            }
    
    /**
     * 设置家庭编号     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFamilyNumber(int familyNumber) {
     	         	    this.familyNumber = familyNumber;
     	        }
    
        [DataMember(Order = 2)]
    private ExamplePerson father;
    
        /**
       * @return 父亲对象，可以为空
    */
        public ExamplePerson getFather() {
               	return father;
            }
    
    /**
     * 设置父亲对象，可以为空     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setFather(ExamplePerson father) {
     	         	    this.father = father;
     	        }
    
        [DataMember(Order = 3)]
    private ExamplePerson mother;
    
        /**
       * @return 母亲对象，可以为空
    */
        public ExamplePerson getMother() {
               	return mother;
            }
    
    /**
     * 设置母亲对象，可以为空     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMother(ExamplePerson mother) {
     	         	    this.mother = mother;
     	        }
    
        [DataMember(Order = 4)]
    private ExamplePerson[] children;
    
        /**
       * @return 孩子列表
    */
        public ExamplePerson[] getChildren() {
               	return children;
            }
    
    /**
     * 设置孩子列表     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setChildren(ExamplePerson[] children) {
     	         	    this.children = children;
     	        }
    
        [DataMember(Order = 5)]
    private ExampleCar[] ownedCars;
    
        /**
       * @return 拥有的汽车信息
    */
        public ExampleCar[] getOwnedCars() {
               	return ownedCars;
            }
    
    /**
     * 设置拥有的汽车信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setOwnedCars(ExampleCar[] ownedCars) {
     	         	    this.ownedCars = ownedCars;
     	        }
    
        [DataMember(Order = 6)]
    private ExampleHouse myHouse;
    
        /**
       * @return 所住的房屋信息
    */
        public ExampleHouse getMyHouse() {
               	return myHouse;
            }
    
    /**
     * 设置所住的房屋信息     *
     * 参数示例：<pre></pre>     
             * 此参数必填
          */
    public void setMyHouse(ExampleHouse myHouse) {
     	         	    this.myHouse = myHouse;
     	        }
    
    
  }
}