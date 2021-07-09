#import <Foundation/Foundation.h>
#import "MSACExceptionModel.h"

@interface MSACExceptionInternal : MSACExceptionModel

@property(nonatomic) NSArray<MSACExceptionInternal *> *innerExceptions;
@property(nonatomic, copy) NSString *wrapperSdkName;

@end
